using MySqlConnector;

namespace Survey;

public class SurveyModel
{
    public static string DBName { get; set; }




    //"Server=localhost;User ID=root;Password=;"

    public static async Task CreateDatabase()
    {
        using MySqlConnection connection = new("Server=192.168.1.15;User ID=ethan;Password=w8Q1Ji8I23s2r4YIsocemabAb5nEQo;");
        await connection.OpenAsync();

        using MySqlCommand cmdCreateDatabase = new($"CREATE DATABASE IF NOT EXISTS `{DBName}`;", connection);
        await cmdCreateDatabase.ExecuteNonQueryAsync();

        await new MySqlCommand($"USE `{DBName}`;", connection).ExecuteNonQueryAsync();

        await new MySqlCommand("CREATE TABLE IF NOT EXISTS user(userID INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,`first_name` VARCHAR(25) NOT NULL,`last_name` VARCHAR(25),`age` INT UNSIGNED,`sex` VARCHAR(18),`height` INT UNSIGNED);", connection).ExecuteNonQueryAsync();

        await new MySqlCommand("CREATE TABLE IF NOT EXISTS question(questionID INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,`question` TEXT NOT NULL);", connection).ExecuteNonQueryAsync();

        await new MySqlCommand("CREATE TABLE IF NOT EXISTS answer(answerID INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,`userID` INT UNSIGNED NOT NULL,`questionID` INT UNSIGNED NOT NULL,`answer` INT UNSIGNED NOT NULL);", connection).ExecuteNonQueryAsync();

        await new MySqlCommand("ALTER TABLE answer ADD CONSTRAINT `FK_userID` FOREIGN KEY IF NOT EXISTS (`userID`) REFERENCES `user` (`userID`);", connection).ExecuteNonQueryAsync();

        await new MySqlCommand("ALTER TABLE answer ADD CONSTRAINT `FK_questionID` FOREIGN KEY IF NOT EXISTS (`questionID`) REFERENCES `question` (`questionID`);", connection).ExecuteNonQueryAsync();

        //           await new MySqlCommand("INSERT INTO `v03_survey`.`question` (`question`) VALUES('test1');", connection).ExecuteNonQueryAsync();
    }



    public static async Task<uint> GetNumberQuestion()
    {
        using MySqlConnection connection = new($"Server=192.168.1.15;User ID=ethan;Password=w8Q1Ji8I23s2r4YIsocemabAb5nEQo;DataBase={DBName}");
        await connection.OpenAsync();

        var numQuestion = await new MySqlCommand("SELECT COUNT(*) FROM question;", connection).ExecuteScalarAsync();

        if (numQuestion != null)
        {
            var numQ = uint.Parse(numQuestion.ToString());
            return numQ;
        }
        return 0;
    }

    public static async Task AddQuestions(IEnumerable<string> questions)
    {
        using MySqlConnection connection = new($"Server=192.168.1.15;User ID=ethan;Password=w8Q1Ji8I23s2r4YIsocemabAb5nEQo;DataBase={DBName}");
        await connection.OpenAsync();


        foreach (var question in questions)
        {
            var cmd = new MySqlCommand($"INSERT INTO `{DBName}`.`question` (`question`) VALUES(@qText);", connection);
            cmd.Parameters.AddWithValue("qText", question);
            await cmd.ExecuteNonQueryAsync();
        }
    }


    public static async Task<uint> AddUser(string first, string last, uint age, string gender, float height)
    {
        using MySqlConnection connection = new($"Server=192.168.1.15;User ID=ethan;Password=w8Q1Ji8I23s2r4YIsocemabAb5nEQo;DataBase={DBName}");
        await connection.OpenAsync();

        var cmd = new MySqlCommand($"INSERT INTO `{DBName}`.`user` (`first_name`, `last_name`, `age`, `sex`, `height`) VALUES(@first, @last, @age, @sex, @height);", connection);
        cmd.Parameters.AddWithValue("first", first);
        cmd.Parameters.AddWithValue("last", last);
        cmd.Parameters.AddWithValue("age", age);
        cmd.Parameters.AddWithValue("sex", gender);
        cmd.Parameters.AddWithValue("height", height);
        await cmd.ExecuteNonQueryAsync();

        return (uint)cmd.LastInsertedId;
    }


    public static async Task<uint> AddAnswer(uint userId, uint questionId, uint answer)
    {
        using MySqlConnection connection = new($"Server=192.168.1.15;User ID=ethan;Password=w8Q1Ji8I23s2r4YIsocemabAb5nEQo;DataBase={DBName}");
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
        using MySqlConnection connection = new($"Server=192.168.1.15;User ID=ethan;Password=w8Q1Ji8I23s2r4YIsocemabAb5nEQo;DataBase={DBName}");
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