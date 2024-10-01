using Newtonsoft.Json;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Exceptions;
using OnDijon.Common.Utils.Extensions;
using OnDijon.Common.Entities.Dto;
using OnDijon.Common.Entities.Request;
using OnDijon.Common.Entities.Response;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnDijon.Common.Utils.Services
{
    public class HttpService : Interfaces.IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Publics methods

        #region Get
        public async Task<T> GetAsync<T>(string path, HttpMethod method, string jsonContent) where T : class
        {
            try { 
                var message = new HttpRequestMessage(); 
                if (!string.IsNullOrEmpty(jsonContent)) 
                { 
                    message.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"); 
                } 
                message.Method = method;
                message.RequestUri = new Uri(path); 
                var reponse = await this._httpClient.SendAsync(message); 
                if (!reponse.IsSuccessStatusCode) 
                { 
                    return null; 
                } 
                var content = await reponse.Content.ReadAsStringAsync(); 
                var resultObj = JsonConvert.DeserializeObject<T>(content); 
                return resultObj; 
            } 
            catch (Exception e) 
            { 
                Console.WriteLine(e); 
                return null; 
            }
        }

        public async Task<T> GetBisAsync<T>(string path, HttpMethod method) where T : class
        {
            Console.WriteLine("Pat = " + path + method);
            try
            {
                var message = new HttpRequestMessage();
                
                message.Method = method;
                message.RequestUri = new Uri(path);
                var reponse = await this._httpClient.SendAsync(message);
                if (!reponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("erreur httprequest " + reponse.StatusCode);
                    return null;
                }
                var content = await reponse.Content.ReadAsStringAsync();
                var resultObj = JsonConvert.DeserializeObject<T>(content);
                return resultObj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        #endregion

        #endregion

        #region Post

        #region DIJON METROPOLE
        public async Task<DtoResponse<T>> PostAsync<T>(Uri path, DtoRequest request) where T : Dto, new()
        {
            HttpRequestMessage requestMessage = BuildRequestMessage(path, HttpMethod.Post, request.ToString());

            var timeoutSource = new CancellationTokenSource(Constants.TIMEOUT);

            try
            {
                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage, timeoutSource.Token);
                T data = await Dto.FromHttpContentAsync<T>(response.Content);
                return new DtoResponse<T> { State = CallStatusEnum.Success, Data = data };
            }
            catch (HttpStatusCodeException ex)
            {
                Console.WriteLine(ex.ToString());
                return new DtoResponse<T> { State = ex.StatusCode.ToCallStatus(), Message = ex.ToString() };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new DtoResponse<T> { State = CallStatusEnum.UnknownError, Message = ex.ToString() };
            }
        }

        public async Task<T> PostAsync<T>(Uri path, string jsonContent) where T : WsDMDto, new()
        {
            T result = new T();

            CancellationTokenSource source = new CancellationTokenSource(Constants.TIMEOUT);

            HttpRequestMessage requestMessage = BuildRequestMessage(path, HttpMethod.Post, jsonContent);
            requestMessage.Headers.Add("Build-Configuration", Constants.BUILD_CONFIGURATION);

            try
            {
                var response = await _httpClient.SendAsync(requestMessage, source.Token);
                var json = response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(json.Result))
                        result = JsonConvert.DeserializeObject<T>(json.Result);
                }
                else
                {
                    result.Message = json.Result;
                    result.StatusCodes = new System.Collections.Generic.List<string>() { response.StatusCode.ToString() };
                }
                result.StatusCode = response.StatusCode;
            }
            catch (Exception ex)
            {
                var error = ex;
                Debug.WriteLine(error);
            }

            return result;
        }


     

        public async Task<T> PostBisAsync<T>(string path, HttpMethod method, string jsonContent) where T : class
        {
            try
            {
                var message = new HttpRequestMessage();
                if (!string.IsNullOrEmpty(jsonContent))
                {
                    Console.WriteLine(jsonContent);
                    message.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                }
                message.Method = method;
                message.RequestUri = new Uri(path);
                Console.WriteLine(path);
                var reponse = await this._httpClient.SendAsync(message);
                if (!reponse.IsSuccessStatusCode)
                {
                    return null;
                }
                var content = await reponse.Content.ReadAsStringAsync();
                var resultObj = JsonConvert.DeserializeObject<T>(content);
                return resultObj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        #endregion

        #region Update
        public async Task<T> PutAsync<T>(string path, HttpMethod method, string jsonContent) where T : class
        {
            try
            {
                var message = new HttpRequestMessage();
                if (!string.IsNullOrEmpty(jsonContent))
                {
                    message.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                }
                message.Method = method;
                message.RequestUri = new Uri(path);
                var response = await this._httpClient.SendAsync(message);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var content = await response.Content.ReadAsStringAsync();
                var resultObj = JsonConvert.DeserializeObject<T>(content);
                return resultObj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }


        #endregion

        #region Delete

        public async Task<T> DeleteAsync<T>(string path, HttpMethod method, string jsonContent) where T : class
        {

            try
            {
                var message = new HttpRequestMessage();
                if (!string.IsNullOrEmpty(jsonContent))
                {
                    message.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                }
                message.Method = method;
                message.RequestUri = new Uri(path);
                var reponse = await this._httpClient.SendAsync(message);
                if (!reponse.IsSuccessStatusCode)
                {
                    return null;
                }
                var content = await reponse.Content.ReadAsStringAsync();
                var resultObj = JsonConvert.DeserializeObject<T>(content);
                return resultObj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        #endregion


        #region Privates methods

        private HttpRequestMessage BuildRequestMessage(Uri targetAddress, HttpMethod method, string jsonContent)
        {
            HttpRequestMessage request = new HttpRequestMessage { Method = method, RequestUri = targetAddress };

            if (!string.IsNullOrEmpty(jsonContent))
            {
                request.Content = new StringContent(jsonContent);
                request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json") { CharSet = Encoding.UTF8.WebName };
            }
            return request;
        }

        #endregion

    }
    #endregion
}