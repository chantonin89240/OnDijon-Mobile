using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.Account.Entities.Response
{
    public class AccountResponse<T> : DtoResponse<T> where T : WsDMDto
    {
        public AccountResponse(DtoResponse<T> dtoResponse)
        {
            if (!ParseAccountDto(dtoResponse.Data))
            {
                State = dtoResponse.State;
                Message = dtoResponse.Message;
            }

            Data = dtoResponse.Data;
        }

        public List<AccountStatusCode> AccountStatusCodes { get; set; }

        /// <summary>
        /// Parse AccountDto from DtoResponse.
        /// </summary>
        /// <param name="accountDto"></param>
        /// <returns>true if data was parsed</returns>
        private bool ParseAccountDto(WsDMDto accountDto)
        {
            if (accountDto != null && accountDto.StatusCodes.Any())
            {
                AccountStatusCodes = new List<AccountStatusCode>();
                foreach (var statusCode in accountDto.StatusCodes)
                {
                    var accountStatus = (AccountStatusCode)Enum.Parse(typeof(AccountStatusCode), statusCode);
                    AccountStatusCodes.Add(accountStatus);
                }

                State = AccountStatusCodes.First().ToCallStatus();
                Message = accountDto.StatusMessages
                    .Select(dto => dto.Value)
                    .Aggregate("", (message, next) => message + next + "\n");

                return true;
            }

            return false;
        }
    }
}