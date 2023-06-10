using System;
using System.Globalization;
using System.Speech.Recognition;

class Program
{
    static void Main(string[] args)
    {
        CultureAndRegionInfoBuilder car1 = new CultureAndRegionInfoBuilder("ew-EW",
                                           CultureAndRegionModifiers.None);
        car1.LoadDataFromCultureInfo(CultureInfo.CreateSpecificCulture("ew-EW"));
        car1.LoadDataFromRegionInfo(new RegionInfo("fr-FR"));

        car1.CultureEnglishName = "TOGO (EWE)";
        car1.CultureNativeName = "русский (США)";
        car1.CurrencyNativeName = "Доллар (США)";
        car1.RegionNativeName = "EWE";

        // Register the culture.
        try
        {
            car1.Register();
        }
        catch (InvalidOperationException)
        {
            // Swallow the exception: the culture already is registered.
        }

        // Création de l'instance du moteur de reconnaissance vocale
        // Création de l'instance du moteur de reconnaissance vocale
        SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();

        // Configuration des paramètres de reconnaissance
        recognizer.SetInputToDefaultAudioDevice(); // Utilisation du périphérique audio par défaut
        recognizer.LoadGrammar(CreateGrammarFromFile()); // Chargement de la grammaire de dictée

        // Définition des gestionnaires d'événements de reconnaissance
        recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        recognizer.SpeechRecognitionRejected += Recognizer_SpeechRecognitionRejected;

        // Démarrage de la reconnaissance vocale
        recognizer.RecognizeAsync(RecognizeMode.Multiple);

        // Attente de l'arrêt de l'application
        Console.WriteLine("Assistant vocal en cours... Appuyez sur une touche pour arrêter.");
        Console.ReadKey();

        // Arrêt de la reconnaissance vocale
        recognizer.Dispose();
    }
    static Grammar CreateGrammarFromFile()
    {
        Grammar citiesGrammar = new Grammar(@"D:\Visual studio\EweTest\EweTest\EweGrammar.xml");
        citiesGrammar.Name = "SRGS File Cities Grammar";
        return citiesGrammar;
    }
    static void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
        Console.WriteLine("Recognized text: " + e.Result.Text);
        if (e.Result.Confidence >= 0.7)
        {
            Console.WriteLine("Commande vocale reconnue : " + e.Result.Text);
        }
    }

    static void Recognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
    {
        Console.WriteLine("La reconnaissance vocale a été rejetée.");
    }
}
