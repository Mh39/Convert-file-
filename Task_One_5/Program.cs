using Newtonsoft.Json;

namespace Task_One_5
{
    public class Program
    {
        public class person
        {
            public person() { }

            public person(int age, string country, string name, string otherData)
            {
                Age = age;
                Country = country;
                Name = name;
                OtherData = otherData;
            }

            public int Age { get; set; }
            public string Country { get; set; }
            public string Name { get; set; }
            public string OtherData { get; set; }
        }
        static void Main(string[] args)
        {
            ConvertFile($@"E:\tit\Farag\conver File\{DateTime.Now:dd-MM-yyyy}.M", @"E:\tit\Farag\conver File\newJsonFile.json");

        }

        //1- Check File Exist
        public static bool CheckFileExist(string filepath)
        {
            return File.Exists(filepath);
        }
        //2- Check Correct File name
        public static bool CheckCorrectFileName(string filepath)
        {
            var fileName = Path.GetFileName(filepath);
            var fileNameToCompare = $@"{DateTime.Now:dd-MM-yyyy}.M";
            return fileNameToCompare.Equals(fileName);
        }
        //3- Read All lines inside File
        public static string[] ReadFileLines(string filepath)
        {
            return File.ReadAllLines(filepath);
        }
        //4-Check First Line
        public static bool CheckFirstLine(string line, int count)
        {
            if (string.IsNullOrWhiteSpace(line))
                return false;
            //first method
            char[] delimiterChars = { ':', ',' };
            string[] data = line.Split(delimiterChars);
            if (data.Length != 4)
                return false;
            if (data[1] != DateTime.Now.ToString("dd-MM-yyyy"))
                return false;
            if (data[3] != count.ToString())
                return false;
            return true;
            //second method
            //var stringToCompare = $@"date:{DateTime.Now:dd-MM-yyy},count:{count}";
            //return stringToCompare.Equals(line);
        }
        //5-Check IsLineValid
        public static bool IsLineValid(string line)
        {
            var data = line.Split(',');
            if (data.Length != 4)
                return false;
            if (!int.TryParse(data[0], out _))
                return false;

            return true;
        }

        //6-Check second line
        public static bool CheckSecondLine(int age, string country, string name, string otherdata)
        {
            if (age <= 0 || age > 120)
                return false;
            if (country == null)
                return false;
            if (name == null)
                return false;
            return true;
        }

        //7- Method to fill data
        public static List<person> FillData(string[] fileData)
        {
            var persons = new List<person>();

            for (int i = 1; i < fileData.Length; i++)
            {
                var data = fileData[i].Split(',');
                CheckSecondLine(Convert.ToInt32(data[0]), data[1], data[2], data[3]);
                persons.Add(new person(Convert.ToInt32(data[0]), data[1], data[2], data[3]));
            }
            return persons;
        }

        //8- Write data to json file
        public static void WriteDataToJsonFile(string outPath, List<person> personsList)
        {
            var json = JsonConvert.SerializeObject(personsList);
            File.WriteAllText(outPath, json);
        }
        //9-Convert Text file to json file
        public static void ConvertFile(string inFilePath, string outFilePath)
        {
            if (!CheckFileExist(inFilePath))
                throw new Exception("File Not Found");

            if (!CheckCorrectFileName(inFilePath))
                throw new Exception("incorrect file name");

            var fileData = ReadFileLines(inFilePath);
            if (fileData.Length == 0)
                throw new Exception("File empty");

            if (!CheckFirstLine(fileData[0], fileData.Length - 1))
                throw new Exception("invalid first line");

            for (int i = 1; i < fileData.Length; i++)
            {
                if (!IsLineValid(fileData[i]))
                    throw new Exception($"invalid line data {i}");
            }

            var persons = FillData(fileData);
            WriteDataToJsonFile(outFilePath, persons);
            Console.WriteLine("Convert Successful");
        }

    }
}