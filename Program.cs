using System;
using OSGeo.GDAL;
using OSGeo.OSR;

namespace LimitesTif
{
    class Program{

        static public void Main(){
            
            GdalConfiguration.ConfigureGdal();
            GdalConfiguration.ConfigureOgr();

            limites("C:\\Users\\luizg\\Downloads\\MINA_JUNHO_23.tif");
        }

        static void limites(string caminhoParaOArquivoTif){

           using (Dataset dataset = Gdal.Open(caminhoParaOArquivoTif, Access.GA_ReadOnly))
            {
                double[] geoTransform = new double[6];
                dataset.GetGeoTransform(geoTransform);

                int largura = dataset.RasterXSize;
                int altura = dataset.RasterYSize;

                double x1 = geoTransform[0];
                double y1 = geoTransform[3];
                double x2 = geoTransform[0] + largura * geoTransform[1] + altura * geoTransform[2];
                double y2 = geoTransform[3] + largura * geoTransform[4] + altura * geoTransform[5];

                SpatialReference sr = new SpatialReference(dataset.GetProjection());
                SpatialReference srLatLong = new SpatialReference("");
                srLatLong.ImportFromEPSG(4326);

                CoordinateTransformation transformation = new CoordinateTransformation(sr, srLatLong);

                double[] coordenadasPonto1 = { x1, y1, 0 };
                double[] coordenadasPonto2 = { x2, y1, 0 };
                double[] coordenadasPonto3 = { x2, y2, 0 };
                double[] coordenadasPonto4 = { x1, y2, 0 };

                transformation.TransformPoint(coordenadasPonto1);
                transformation.TransformPoint(coordenadasPonto2);
                transformation.TransformPoint(coordenadasPonto3);
                transformation.TransformPoint(coordenadasPonto4);

                Console.WriteLine("Coordenadas geográficas dos pontos limites:");
                Console.WriteLine("Ponto Superior Esquerdo (Longitude, Latitude): " + coordenadasPonto1[0] + ", " + coordenadasPonto1[1]);
                Console.WriteLine("Ponto Superior Direito (Longitude, Latitude): " + coordenadasPonto2[0] + ", " + coordenadasPonto2[1]);
                Console.WriteLine("Ponto Inferior Direito (Longitude, Latitude): " + coordenadasPonto3[0] + ", " + coordenadasPonto3[1]);
                Console.WriteLine("Ponto Inferior Esquerdo (Longitude, Latitude): " + coordenadasPonto4[0] + ", " + coordenadasPonto4[1]);
            }
        }
    }

}