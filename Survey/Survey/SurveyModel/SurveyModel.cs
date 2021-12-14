using MySqlConnector;

namespace Survey;

public class SurveyModel
{
    public static string DBName { get; set; }

    public static async Task CreateDatabase()
    {
        using MySqlConnection connection = new("Server=localhost;User ID=root;Password=;");
        await connection.OpenAsync();

        using MySqlCommand cmdCreateDatabase = new($"CREATE DATABASE IF NOT EXISTS `{DBName}`;", connection);
        await cmdCreateDatabase.ExecuteNonQueryAsync();

        await new MySqlCommand($"USE `{DBName}`;", connection).ExecuteNonQueryAsync();

        await new MySqlCommand("CREATE TABLE IF NOT EXISTS user(userID INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,`first_name` VARCHAR(25) NOT NULL,`last_name` VARCHAR(25) NOT NULL,`age_group` VARCHAR(30) NOT NULL,`gender` VARCHAR(10) NOT NULL,`email` VARCHAR(128) NOT NULL);", connection).ExecuteNonQueryAsync();

        await new MySqlCommand("CREATE TABLE IF NOT EXISTS question(questionID INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,`question` TEXT NOT NULL);", connection).ExecuteNonQueryAsync();

        await new MySqlCommand("CREATE TABLE IF NOT EXISTS answer(answerID INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,`userID` INT UNSIGNED NOT NULL,`questionID` INT UNSIGNED NOT NULL,`answer` INT UNSIGNED NOT NULL);", connection).ExecuteNonQueryAsync();

        await new MySqlCommand("ALTER TABLE answer ADD CONSTRAINT `FK_userID` FOREIGN KEY IF NOT EXISTS (`userID`) REFERENCES `user` (`userID`);", connection).ExecuteNonQueryAsync();

        await new MySqlCommand("ALTER TABLE answer ADD CONSTRAINT `FK_questionID` FOREIGN KEY IF NOT EXISTS (`questionID`) REFERENCES `question` (`questionID`);", connection).ExecuteNonQueryAsync();

        //           await new MySqlCommand("INSERT INTO `v03_survey`.`question` (`question`) VALUES('test1');", connection).ExecuteNonQueryAsync();
    }


    public static async Task<uint> GetNumberQuestion()
    {
        using MySqlConnection connection = new($"Server=localhost;User ID=root;Password=;DataBase={DBName}");
        await connection.OpenAsync();

        var numQuestion = await new MySqlCommand("SELECT COUNT(*) FROM question;", connection).ExecuteScalarAsync();

        if (numQuestion != null)
        {
            var numQ = uint.Parse(numQuestion.ToString());
            return numQ;
        }
        return 0;
    }


    public static async Task<List<int>> GetGenderResults()
    {
        using MySqlConnection connection = new($"Server=localhost;User ID=root;Password=;DataBase={DBName}");
        await connection.OpenAsync();

        List<int> genderResults = new();

        var noOfMale = await new MySqlCommand("SELECT COUNT(*) FROM user WHERE gender LIKE 'male';", connection).ExecuteScalarAsync();
        var noOfFemale = await new MySqlCommand("SELECT COUNT(*) FROM user WHERE gender LIKE 'female';", connection).ExecuteScalarAsync();

        genderResults.Add(int.Parse(noOfMale.ToString()));
        genderResults.Add(int.Parse(noOfFemale.ToString()));

        return genderResults;
    }

    public static async Task<List<int>> GetAgeGroupResults()
    {
        using MySqlConnection connection = new($"Server=localhost;User ID=root;Password=;DataBase={DBName}");
        await connection.OpenAsync();

        List<int> ageGroupResults = new();

        var ag1 = await new MySqlCommand("SELECT COUNT(*) FROM user WHERE age_group LIKE '18-24 years old';", connection).ExecuteScalarAsync();
        var ag2 = await new MySqlCommand("SELECT COUNT(*) FROM user WHERE age_group LIKE '25-34 years old';", connection).ExecuteScalarAsync();
        var ag3 = await new MySqlCommand("SELECT COUNT(*) FROM user WHERE age_group LIKE '35-44 years old';", connection).ExecuteScalarAsync();
        var ag4 = await new MySqlCommand("SELECT COUNT(*) FROM user WHERE age_group LIKE '45-54 years old';", connection).ExecuteScalarAsync();
        var ag5 = await new MySqlCommand("SELECT COUNT(*) FROM user WHERE age_group LIKE '55-64 years old';", connection).ExecuteScalarAsync();
        var ag6 = await new MySqlCommand("SELECT COUNT(*) FROM user WHERE age_group LIKE '65-74 years old';", connection).ExecuteScalarAsync();
        var ag7 = await new MySqlCommand("SELECT COUNT(*) FROM user WHERE age_group LIKE '75 years or older';", connection).ExecuteScalarAsync();

        ageGroupResults.Add(int.Parse(ag1.ToString()));
        ageGroupResults.Add(int.Parse(ag2.ToString()));
        ageGroupResults.Add(int.Parse(ag3.ToString()));
        ageGroupResults.Add(int.Parse(ag4.ToString()));
        ageGroupResults.Add(int.Parse(ag5.ToString()));
        ageGroupResults.Add(int.Parse(ag6.ToString()));
        ageGroupResults.Add(int.Parse(ag7.ToString()));

        return ageGroupResults; 
    }



    public static async Task AddQuestions(IEnumerable<string> questions)
    {
        using MySqlConnection connection = new($"Server=localhost;User ID=root;Password=;DataBase={DBName}");
        await connection.OpenAsync();


        foreach (var question in questions)
        {
            var cmd = new MySqlCommand($"INSERT INTO `{DBName}`.`question` (`question`) VALUES(@qText);", connection);
            cmd.Parameters.AddWithValue("qText", question);
            await cmd.ExecuteNonQueryAsync();
        }
    }


    public static async Task<uint> AddUser(string first, string last, string age_group, string gender, string email)
    {
        using MySqlConnection connection = new($"Server=localhost;User ID=root;Password=;DataBase={DBName}");
        await connection.OpenAsync();

        var cmd = new MySqlCommand($"INSERT INTO `{DBName}`.`user` (`first_name`, `last_name`, `age_group`, `gender`, `email`) VALUES(@first, @last, @age_group, @gender, @email);", connection);
        cmd.Parameters.AddWithValue("first", first);
        cmd.Parameters.AddWithValue("last", last);
        cmd.Parameters.AddWithValue("age_group", age_group);
        cmd.Parameters.AddWithValue("gender", gender);
        cmd.Parameters.AddWithValue("email", email);
        await cmd.ExecuteNonQueryAsync();

        return (uint)cmd.LastInsertedId;
    }


    public static async Task<uint> AddAnswer(uint userId, uint questionId, uint answer)
    {
        using MySqlConnection connection = new($"Server=localhost;User ID=root;Password=;DataBase={DBName}");
        await connection.OpenAsync();

        var cmd = new MySqlCommand($"INSERT INTO `{DBName}`.`answer` (`userID`, `questionID`, `answer`) VALUES(@userId, @questionId, @answer);", connection);
        cmd.Parameters.AddWithValue("userID", userId);
        cmd.Parameters.AddWithValue("questionID", questionId);
        cmd.Parameters.AddWithValue("answer", answer);
        await cmd.ExecuteNonQueryAsync();

        return (uint)cmd.LastInsertedId;
    }



    public static async Task<IEnumerable<DBQuestion>> GetQuestions()
    {
        using MySqlConnection connection = new($"Server=localhost;User ID=root;Password=;DataBase={DBName}");
        await connection.OpenAsync();

        var q = new MySqlCommand("SELECT questionID, question from question;", connection);
        using var res = await q.ExecuteReaderAsync();

        var questions = new List<DBQuestion>();

        while (res.Read())
        {
            questions.Add(new DBQuestion { QuestionId = res.GetUInt32(0), Question = res.GetString(1) });
        }

        return questions;
    }

}

public class DBQuestion
{
    public uint QuestionId { get; set; }
    public string Question { get; set; }

    public override string ToString()
    {
        return Question;
    }
}