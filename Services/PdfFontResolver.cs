using System.Collections.Generic;
using System.IO;
using PdfSharp.Fonts;

namespace SistemaGestionNomina.Services
{
    public class PdfFontResolver : IFontResolver
    {
        private const string RegularFace = "SegoeUI#Regular";
        private const string BoldFace = "SegoeUI#Bold";
        private static bool registered;

        public static void EnsureRegistered()
        {
            if (!registered && GlobalFontSettings.FontResolver == null)
            {
                GlobalFontSettings.FontResolver = new PdfFontResolver();
                registered = true;
            }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            return new FontResolverInfo(isBold ? BoldFace : RegularFace);
        }

        public byte[] GetFont(string faceName)
        {
            Dictionary<string, string[]> candidates = new Dictionary<string, string[]>();
            candidates[RegularFace] = new[] { "segoeui.ttf", "arial.ttf" };
            candidates[BoldFace] = new[] { "segoeuib.ttf", "arialbd.ttf", "arial.ttf" };

            string windowsFonts = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows), "Fonts");
            string[] files = candidates.ContainsKey(faceName) ? candidates[faceName] : candidates[RegularFace];
            for (int i = 0; i < files.Length; i++)
            {
                string path = Path.Combine(windowsFonts, files[i]);
                if (File.Exists(path))
                {
                    return File.ReadAllBytes(path);
                }
            }

            throw new FileNotFoundException("No se encontró una fuente compatible para generar el PDF.");
        }
    }
}
