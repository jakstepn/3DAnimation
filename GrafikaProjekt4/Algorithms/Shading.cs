using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKtoolkit
{
    public static class Shading
    {
        public static Color Intensity(double ks, double kd, double ka, int shininess,
            Position point, CustomVector normal, List<LightSource> lights, ICamera camera, Color pixelColor)
        {
            double pixelRed = pixelColor.R / 255.0;
            double pixelGreen = pixelColor.G / 255.0;
            double pixelBlue = pixelColor.B / 255.0;

            double iared = Ia(pixelRed);
            double iagreen = Ia(pixelGreen);
            double iablue = Ia(pixelBlue);

            double colorSumBlue = 0;
            double colorSumGreen = 0;
            double colorSumRed = 0;

            foreach (LightSource lightSource in lights)
            {
                double lightRed = lightSource.color.R / 255.0;
                double lightGreen = lightSource.color.G / 255.0;
                double lightBlue = lightSource.color.B / 255.0;

                double distance = lightSource.DistanceTo(point);
                double If = 1.0 / (1 + 0.09 * distance + 0.032 * distance * distance);
                CustomVector toCamHat = new CustomVector(camera.position, point).Normalized();
                CustomVector toLightHat = new CustomVector(lightSource.position, point).Normalized();

                colorSumRed += (kd * Id(lightRed * pixelRed, normal, toLightHat) +
                    ks * Is(lightRed * pixelRed, normal, toLightHat, toCamHat, shininess)) * If;

                colorSumGreen += (kd * Id(lightGreen * pixelGreen, normal, toLightHat) +
                    ks * Is(lightGreen * pixelGreen, normal, toLightHat, toCamHat, shininess)) * If;

                colorSumBlue += (kd * Id(lightBlue * pixelBlue, normal, toLightHat) +
                    ks * Is(lightBlue * pixelBlue, normal, toLightHat, toCamHat, shininess)) * If;
            }
            colorSumRed = Math.Max(colorSumRed, 0);
            colorSumGreen = Math.Max(colorSumGreen, 0);
            colorSumBlue = Math.Max(colorSumBlue, 0);

            return Color.FromArgb(255,
    (byte)(Math.Min(ka * iared + colorSumRed, 1) * 255),
    (byte)(Math.Min(ka * iagreen + colorSumGreen, 1) * 255),
    (byte)(Math.Min(ka * iablue + colorSumBlue, 1) * 255));
        }

        private static double Ia(double pixelColor)
        {
            return pixelColor;
        }
        private static double Id(double lightColor, CustomVector normal, CustomVector toLightHat)
        {
            return lightColor * (normal | toLightHat);
        }
        private static double Is(double lightColor, CustomVector normal, CustomVector toLightHat,
            CustomVector toCamVectorHat, int shininess)
        {
            CustomVector Rhat = (2 * (toLightHat | normal) * normal - toLightHat).Normalized();
            return lightColor * ToPower(shininess, (Rhat | toCamVectorHat));
        }

        private static double ToPower(int power, double value)
        {
            if (power == 0)
            {
                return 1;
            }
            double sum = value;
            for (int i = 1; i < power; i++)
            {
                sum *= value;
            }
            return sum;
        }
    }
}
