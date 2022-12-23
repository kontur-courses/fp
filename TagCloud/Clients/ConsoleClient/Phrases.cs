namespace TagCloud.Clients.ConsoleClient;

public class Phrases
{
    public const string TryAgain = "Повторить попытку? [y/n]: ";
    public const string Yes = "y";

    public const string AskingFontSize =
        "Введите минимальный и максимальный размеры шрифта, которые хотите видеть, через пробел: ";

    public static readonly string Hello = "Вас приветствует создатель облака тегов." + Environment.NewLine +
                                          "Для моей работы нужен файл со словами для облака, которые записаны в столбик." +
                                          Environment.NewLine +
                                          "Так же Вы можете указать список скучных по вашему мнению слов, " +
                                          "которые нужно исключить из построения облака. Их так же нужно указать в файле " +
                                          "по одномму слову в каждой стрроке.";

    public static readonly string AskingFullPathToOutImage =
        Environment.NewLine +
        "Пожалуйста, введите полный путь к файлу, в который необходимо сохранить изображение: ";

    public static readonly string AskingFullPathToText =
        Environment.NewLine +
        "Пожалуйста, введите полный путь к вашему файлу со словами";

    public static readonly string AskingFullPathToBoringWords =
        Environment.NewLine +
        "Пожалуйста, введите полный путь к вашему файлу со словами, коорые соедует исключить при формировании облака.";

    public static readonly string AskingImageSize =
        Environment.NewLine +
        "Пожалуйста, введите размеры изображения, которое хотите получить Ш*В (в пикселях)" +
        Environment.NewLine +
        "Например 800*500: ";

    public static readonly string AskingBgColor =
        Environment.NewLine +
        "Пожалуйста, введите цвет фона на английском. Например white: ";
    
    public static readonly string AskingWordsColors =
        Environment.NewLine +
        "Пожалуйста, введите цвета, в которые необходимо раскрасить слова. Вводите цвета " +
        "от самого редко используемого, к самому часто встречающемуся."
        + Environment.NewLine +
        "Например white-gray-black: ";

    public static readonly string AskingAddingUsersBoringWords =
        Environment.NewLine +
        "Хотите указать файл со скучными словами, которые следует исключить при формировании облака? [y/n]: ";

    public static readonly string SuccessSaveImage = Environment.NewLine + "Файл сохранён успешно в ";

    public static string GetArrow(int indent)
    {
        return new string(' ', indent) + "> ";
    }
}