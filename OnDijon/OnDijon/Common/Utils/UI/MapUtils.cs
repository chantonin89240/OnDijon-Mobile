using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Http;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using Microsoft.AppCenter.Crashes;
using OnDijon.Modules.Report.Entities.Dto;
using OnDijon.Modules.Report.Entities.Request;
using OnDijon.Modules.Report.Entities.Response;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OnDijon.Common.Utils.UI
{
    public static class MapUtils
    {
        public const double DEFAULT_MAP_SCALE = 36000;

        public const double MIN_MAP_SCALE = 1000000;

        /// <summary>
        /// Viewpoint centered on Dijon Métropole with default scale
        /// </summary>
        public static readonly Viewpoint DEFAULT_MAP_VIEWPOINT = new Viewpoint(47.333300, 5.044906, DEFAULT_MAP_SCALE);

        public static readonly Envelope VALID_AREA = new Envelope(4, 46, 6, 48, SpatialReferences.Wgs84);

        public const string REPORT_ID_KEY = "report_id";

        public static async Task<PictureMarkerSymbol> CreatePictureMarkerSymbolFromResources(string resource)
        {
            // Get current assembly that contains the image
            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            // Get image as a stream from the resources
            // Picture is defined as EmbeddedResource and DoNotCopy
            Stream resourceStream = currentAssembly.GetManifestResourceStream(resource);

            // Create new symbol using asynchronous factory method from stream
            return await PictureMarkerSymbol.CreateAsync(resourceStream);
        }

        public static MapPoint ToMapPoint(this Location location)
        {
            return new MapPoint(location.Longitude, location.Latitude, SpatialReferences.Wgs84);
        }

        public static MapPoint ToMapPoint(this CoordinatesRequest coordinates)
        {
            return new MapPoint(coordinates.X, coordinates.Y, SpatialReferences.Wgs84);
        }

        public static MapPoint ToMapPoint(this CoordinatesResponse coordinates)
        {
            return new MapPoint(coordinates.X, coordinates.Y, SpatialReferences.Wgs84);
        }

        public static MapPoint ToMapPoint(this ReportPositionDto coordinates)
        {
            return new MapPoint(coordinates.X, coordinates.Y, SpatialReferences.Wgs84);
        }

        public static CoordinatesRequest ToCoordinatesRequest(this MapPoint mapPoint)
        {
            return new CoordinatesRequest { X = mapPoint.X, Y = mapPoint.Y };
        }

        public static MapPoint ToWgs84(this MapPoint mapPoint)
        {
            return GeometryEngine.Project(mapPoint, SpatialReferences.Wgs84) as MapPoint;
        }

        public static CallStatusEnum ToCallStatus(this ArcGISWebException exception)
        {
            if (Enum.TryParse(exception.Code.ToString(), out HttpStatusCode httpStatus))
            {
                return httpStatus.ToCallStatus();
            }
            else
            {
                return CallStatusEnum.UnknownError;
            }
        }

        public static async Task<Graphic> ToGraphic(this ReportDto report)
        {
            //create report symbol
            var reportSymbol = new CompositeSymbol();
            var circleSymbol = new SimpleMarkerSymbol()
            {
                Color = Color.White,
                Size = 40,
                Style = SimpleMarkerSymbolStyle.Circle,
                Outline = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, report.StatusColor, 3)
            };
            reportSymbol.Symbols.Add(circleSymbol);
            if (!string.IsNullOrEmpty(report.TypeIconUrl))
            {
                try
                {
                    var iconSize = 20;
                    var pngStream = await ImageService.Instance.LoadUrl(report.TypeIconUrl).WithCustomDataResolver(new SvgDataResolver(iconSize, iconSize)).AsPNGStreamAsync();
                    var iconSymbol = await PictureMarkerSymbol.CreateAsync(pngStream);
                    reportSymbol.Symbols.Add(iconSymbol);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }

            //create report graphic with id as attribute
            var reportPosition = report.Position.ToMapPoint();
            var reportAttributes = new Dictionary<string, object> { { REPORT_ID_KEY, report.Id } };
            return new Graphic(reportPosition, reportAttributes, reportSymbol);
        }

        /// <summary>
        /// Show a list of reports on the map and zoom on them
        /// </summary>
        /// <param name="mapView"></param>
        /// <param name="reports"></param>
        /// <param name="reportsOverlay">must be added to the map</param>
        /// <param name="basePoint">a point which will be included in the map viewpoint</param>
        public static async Task ShowReports(this MapView mapView, IList<ReportDto> reports, GraphicsOverlay reportsOverlay, MapPoint basePoint = null)
        {
            reportsOverlay.Graphics.Clear();

            if (reports != null && reports.Any())
            {
                foreach (var report in reports)
                {
                    //create report graphic and add it to overlay
                    reportsOverlay.Graphics.Add(await report.ToGraphic());
                }

                //center and zoom map on the area including reports and basePoint
                if (reports.Count > 1 || basePoint != null)
                {
                    var envelopeBuilder = new EnvelopeBuilder(reportsOverlay.Extent);
                    if (basePoint != null)
                    {
                        var projectedBasePoint = GeometryEngine.Project(basePoint, envelopeBuilder.SpatialReference);
                        envelopeBuilder.UnionOf(projectedBasePoint.Extent);
                    }
                    await mapView.SetViewpointGeometryAsync(envelopeBuilder.Extent, 100);
                }
                else
                {
                    await mapView.SetViewpointCenterAsync(reports[0].Position.ToMapPoint(), DEFAULT_MAP_SCALE);
                }
            }
            else
            {
                //center map on base point or default viewpoint
                var viewpoint = basePoint != null ? new Viewpoint(basePoint.Extent) : DEFAULT_MAP_VIEWPOINT;
                mapView.SetViewpoint(viewpoint);
            }
        }

        public static async Task ShowPinGraphic(this GraphicsOverlay positionOverlay, MapPoint position)
        {
            //create pin symbol
            var pinSymbol = await CreatePictureMarkerSymbolFromResources("OnDijon.Assets.pin.png");
            pinSymbol.Width = 34;
            pinSymbol.Height = 49;
            pinSymbol.OffsetY = pinSymbol.Height / 2;

            //create pin graphic and add it to map overlay
            var pinGraphic = new Graphic(position, pinSymbol);
            positionOverlay.Graphics.Clear();
            positionOverlay.Graphics.Add(pinGraphic);
        }
    }
}
