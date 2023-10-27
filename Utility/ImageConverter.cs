namespace NamNamAPI.Utility;
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Hosting;

public class ImageConverter
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ImageConverter(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public bool Base64ToJpg(string base64Image)
    {
        try
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(webRootPath, "Images", "pruebaImagen");

            byte[] imageBytes = Convert.FromBase64String(base64Image);

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                using (Image image = Image.FromStream(ms))
                {
                    image.Save(imagePath, ImageFormat.Jpeg);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            // Maneja las excepciones según tu lógica de aplicación
            Console.WriteLine("Error al convertir la imagen: " + ex.Message);
            return false;
        }
    }
}