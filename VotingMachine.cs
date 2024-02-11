using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.CodeDom;
using System.Runtime.InteropServices;








///                                                   MUHAMMAD AMIR
///                                                   BSEF21 M013







namespace VotingSystem
{
    internal class VotingMachine
    {
        // 
        
        private List<Candidate> candidates; //  // I couldn't get what this guy was meant for :\
        public VotingMachine() { } 

        

        ///////////////////     SOME PRIVATE UTILITY FUNCTIONS



        private void addObjectToFile(Object o, String filePath)
        {
            // add this voter to text file
            String jsonString = JsonSerializer.Serialize(o);
            StreamWriter sw = new StreamWriter(filePath, true);
            sw.WriteLine(jsonString);
            sw.Close();
            sw.Dispose();
        }
        private void displayObjectsFromFile(string filePath, Type objectType)
        {
            try
            {
                StreamReader sr = new StreamReader(filePath);
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    object deserializedObject = JsonSerializer.Deserialize(line, objectType);

                    // Check if the deserialized object is of the expected type
                    if (deserializedObject != null && objectType.IsAssignableFrom(deserializedObject.GetType()))
                    {
                        // Cast the object to the desired type
                        var castedObject = Convert.ChangeType(deserializedObject, objectType);

                        Console.WriteLine(castedObject);
                    }

                }

                sr.Close();
                sr.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        private void readCandidatesFromDatabase()
        {
            using (SqlConnection conn = new SqlConnection(Config.ConnectionString))
            {
                conn.Open();

                string query = "SELECT * FROM candidate";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = cmd.ExecuteReader();

                    Console.WriteLine("{0,-4} | {1,-10} | {2,-10} | {3,-10}", "ID", "Name", "Party", "Votes");
                    Console.WriteLine(new string('-', 40));  // Separator line

                    while (dr.Read())
                    {
                        Console.WriteLine($"{dr[0],-4} | {dr[1],-10} | {dr[2],-10} | {dr[3],-10}");
                    }
                }
            }
        }
        private void addVoterToDatabase(Voter v)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Voter(VoterID,VoterName) VALUES (@cnic, @name)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameter values to the query
                        command.Parameters.AddWithValue("@cnic", v.CNIC);
                        command.Parameters.AddWithValue("@name", v.VoterName);
                        //command.Parameters.AddWithValue("@selectedpartyname", v.SelectedPartyName);

                        if(command.ExecuteNonQuery() > 0)
                            Console.WriteLine("Voter added to the database successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding Voter ");                
            }
        }
        private void addCandidateToDatabase(Candidate c)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Config.ConnectionString))
                {
                    conn.Open();
                    // default value for vote is 0
                    String query = "insert into Candidate values(@cnic, @name, @party ,@votes)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cnic", c.CandidateID);
                        cmd.Parameters.AddWithValue("@name", c.Name);
                        cmd.Parameters.AddWithValue("@party", c.Party);
                        cmd.Parameters.AddWithValue("@votes", 0);

                        if (cmd.ExecuteNonQuery() > 0)
                            Console.WriteLine("Candidate inserted into Database !");
                    }

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void updateVoterInDatabase(String cnic , String newName)
        {
            SqlConnection conn = new SqlConnection(Config.ConnectionString);
            conn.Open();

            // v has following attributes: 
            // VoterID , VoterName , SelectedPartyName

            String query = "update Voter set VoterName = @arg1 where VoterID=@arg2";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@arg1", newName);
            cmd.Parameters.AddWithValue("@arg2", cnic);
            
            if(cmd.ExecuteNonQuery() > 0 )
            {
                Console.WriteLine("Updated Successfully!");
            }

        }
        private void updatePartyInDB(Voter v, String partyName)
        {
            using (SqlConnection conn = new SqlConnection(Config.ConnectionString))
            {
                conn.Open();

                string query = "UPDATE Voter SET SelectedPartyName = @partyName WHERE VoterID = @cnic";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@partyName", partyName);
                    cmd.Parameters.AddWithValue("@cnic", v.CNIC);

                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        Console.WriteLine("Error Occurred While Casting Vote!");
                    }
                }
            }
        }
        private void updatePartyInFile(Voter v, String partyName)
        {
            try
            {
                StreamReader sr = new StreamReader("voter.txt");
                StreamWriter sw = new StreamWriter("tempVoter.txt");

                string line;
                Voter voter;

                while ((line = sr.ReadLine()) != null)
                {
                    voter = JsonSerializer.Deserialize<Voter>(line);
                    if (voter.CNIC == v.CNIC)
                    {
                        // we will use constructor to set partyName 
                        // since we can't do it directly
                        voter = new Voter(voter.VoterName, voter.CNIC, partyName);
                    }
                    sw.WriteLine(JsonSerializer.Serialize(voter));
                }
                sr.Close();
                sw.Close();
                sr.Dispose();
                sw.Dispose();

                File.Delete("voter.txt");
                File.Move("tempVoter.txt", "voter.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        private void incrementVoteInFile(String FilePath, String candidateID)
        {
            using (StreamReader sr = new StreamReader(FilePath))
            {
                using (StreamWriter sw = new StreamWriter("tempoFile.txt"))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        Candidate c = JsonSerializer.Deserialize<Candidate>(line);

                        if (c.CandidateID == candidateID)
                            c.IncrementVotes();

                        sw.WriteLine(JsonSerializer.Serialize(c));
                    }
                }
            }

            File.Delete(FilePath);
            File.Move("tempoFile.txt", FilePath);
        }
        private void incrementVoteInDB(String cnic)
        {
            using (SqlConnection conn = new SqlConnection(Config.ConnectionString))
            {
                conn.Open();

                string query = "UPDATE Candidate SET Votes = Votes + 1 WHERE CandidateID = @arg1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@arg1", cnic);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception e)
                    {
                        throw e;
                    }

                }
            }
        }
        private void updateVoterInFile(String cnic , String newName)
        {
            // read json from file until this cnic
            StreamReader voterReaderStream = new StreamReader("voter.txt");
            StreamWriter tempWriterStream = new StreamWriter("temp.txt");

            Voter tempVoter = new Voter();

            while (true)
            {
                String line = voterReaderStream.ReadLine();

                if (line == null)
                    break;

                tempVoter = JsonSerializer.Deserialize<Voter>(line);
                if (tempVoter.CNIC == cnic)
                    tempVoter.VoterName = newName; // update the name

                tempWriterStream.WriteLine(JsonSerializer.Serialize(tempVoter));

            }

            // now copy the temp file to voter file

            voterReaderStream.Close();
            tempWriterStream.Close();

            File.Delete("voter.txt");
            File.Move("temp.txt", "voter.txt");
        }
        private void updateCandidateInDB(Candidate c)
        {
            SqlConnection conn = new SqlConnection(Config.ConnectionString);
            conn.Open();

            String query = "update Candidate set Name = @arg1 , Party = @arg2 where CandidateID = @arg3";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@arg1", c.Name);
            cmd.Parameters.AddWithValue("@arg2", c.Party);
            cmd.Parameters.AddWithValue("@arg3", c.CandidateID);

            if (cmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Candidate Updated Successfully!");
            }
        }
        private Candidate readCandidateFromFile(String cnic, bool show = true)
        {
            //  the return type to Candidate for a reason
            StreamReader sr = new StreamReader("candidates.txt");
            String line;
            Candidate candidate;

            while ((line = sr.ReadLine()) != null)
            {
                candidate = JsonSerializer.Deserialize<Candidate>(line);
                if (candidate.CandidateID == cnic)
                {
                    if (show)
                    {
                        Console.WriteLine(candidate);
                    }
                    sr.Close();
                    sr.Dispose();
                    return candidate;
                    break;
                }
            }

            throw new Exception("Candidate doesn't exist.");
        }
        private void deleteCandidateFromFile(String cnic)
        {
            StreamReader sr = new StreamReader("candidates.txt");
            StreamWriter sw = new StreamWriter("tempCandidates.txt");

            String line;
            Candidate candidate;

            while ((line = sr.ReadLine()) != null)
            {
                candidate = JsonSerializer.Deserialize<Candidate>(line);
                if (candidate.CandidateID != cnic)
                {
                    sw.WriteLine(JsonSerializer.Serialize(candidate));
                }
            }
            sr.Close();
            sw.Close();

            File.Delete("candidates.txt");
            File.Move("tempCandidates.txt", "candidates.txt");
        }
        private void deleteVoterFromFile(String cnic)
        {
            StreamReader sr = new StreamReader("voter.txt");
            StreamWriter sw = new StreamWriter("tempVoter.txt");

            String line;
            Voter voter;

            while ((line = sr.ReadLine()) != null)
            {
                voter = JsonSerializer.Deserialize<Voter>(line);
                if (voter.CNIC != cnic)
                {
                    sw.WriteLine(JsonSerializer.Serialize(voter));
                }
            }
            sr.Close();
            sw.Close();

            File.Delete("voter.txt");
            File.Move("tempVoter.txt", "voter.txt");

        }
        private void deleteVoterFromDB(String cnic)
        {
            SqlConnection conn = new SqlConnection(Config.ConnectionString);
            conn.Open();

            string query = "DELETE FROM Voter WHERE VoterID = @arg1";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@arg1", cnic);

            if (cmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Voter Deleted Successfully!");
            }
        }
        private void deleteCandidateFromDB(string cnic)
        {
            SqlConnection conn = new SqlConnection(Config.ConnectionString);
            conn.Open();

            string query = "DELETE FROM Candidate WHERE CandidateID = @arg1";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@arg1", cnic);

            if (cmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Candidate Deleted Successfully!");
            }
        }
        private void updateCandidateInFile(String cnic)
        {
            StreamReader sr = new StreamReader("candidates.txt");
            StreamWriter sw = new StreamWriter("tempCandidates.txt");

            String line;
            Candidate candidate;

            while ((line = sr.ReadLine()) != null)
            {
                candidate = JsonSerializer.Deserialize<Candidate>(line);
                if (candidate.CandidateID == cnic)
                {
                    Console.WriteLine("Enter New Candidate Name: ");
                    candidate.Name = Console.ReadLine();
                    Console.WriteLine("Enter New Party Name: ");
                    candidate.Party = Console.ReadLine();
                    updateCandidateInDB(candidate); // <-- here we are making call to update
                                                    // candidate in database as well
                }
                sw.WriteLine(JsonSerializer.Serialize(candidate));
            }
            sr.Close();
            sr.Dispose();
            sw.Close();
            sw.Dispose();
            File.Delete("candidates.txt");
            File.Move("tempCandidates.txt", "candidates.txt");

        }
        private void readCandidateFromDB(string cnic)
        {
            SqlConnection conn = new SqlConnection(Config.ConnectionString);
            conn.Open();

            string query = "SELECT * FROM Candidate WHERE CandidateID = @arg1";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@arg1", cnic);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                Console.WriteLine("Candidate Details:");
                Console.WriteLine($"| {"ID",-15} | {"Name",-20} | {"Party",-15} | {"Votes",-10} |");
                Console.WriteLine(new string('-', 70));
                Console.WriteLine($"| {dr[0],-15} | {dr[1],-20} | {dr[2],-15} | {dr[3],-10} |");
            }
        }
        private void readVotersFromDatabase()
        {
            string connectionStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"MyDB\";Integrated Security=True;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionStr))
            {
                sqlConnection.Open();

                Console.WriteLine("{0,-4} | {1,-10} | {2,-10}", "ID", "Name", "Party");
                Console.WriteLine(new string('-', 30));  // Separator line

                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Voter", sqlConnection))
                {
                    using (SqlDataReader dr = sqlCommand.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Console.WriteLine($"{dr[0],-4} | {dr[1],-10} | {dr[2],-10}");
                        }
                    }
                }
            }
        }


        ///////////////////////////////    PUBLIC INTERFACE


        public void castVote(Candidate c , Voter v)
        {
            // check if this voter has already casted the vote or not
            if(v.hasVoted(v.CNIC) == true)
            {
                Console.WriteLine("ghar jao beta! ( apne vote de diya ha)");
            }
            else
            {
                // we could use functions updateVoter() and updateCandidate() . But , the way they 
                // are defined won't allow since they are just meant for updating partial info :/

                // since we can't directly set v.selectedParty , use constructor
                //Voter newVoter = new Voter(v.VoterName, v.CNIC, c.Party);

                updatePartyInDB(v, c.Party); 
                updatePartyInFile(v, c.Party);
                incrementVoteInDB(c.CandidateID);                
                incrementVoteInFile("candidates.txt" , c.CandidateID);     
            }
        }

        public void addVoter() 
        { 
            String tempCnic;
            String tempName;
            Console.WriteLine("Enter Voter Name: ");
            tempName = Console.ReadLine();
            Console.WriteLine("Enter Voter CNIC: ");
            tempCnic = Console.ReadLine();
            Voter v = new Voter(tempName, tempCnic, "null"); // null is the default value for selected party
            addObjectToFile(v , "voter.txt");
            addVoterToDatabase(v);
        }
        public void updateVoter(String cnic)
        {
            Console.WriteLine("Enter New Voter Name: ");
            String newName = Console.ReadLine();

            updateVoterInFile(cnic , newName);
            updateVoterInDatabase(cnic, newName);
        }
        public void displayVoters()
        {
            Console.WriteLine("Voters in File: ");
            Console.WriteLine();
            displayObjectsFromFile("voter.txt", typeof(Voter));
            Console.WriteLine();
            Console.WriteLine("Voters in Database: ");
            readVotersFromDatabase();
        }
        public void displayCandidates()
        {
            Console.WriteLine("Candidates From File: ");
            displayObjectsFromFile("candidates.txt", typeof(Candidate));
            Console.WriteLine();
            Console.WriteLine("Candidates From DB");
            readCandidatesFromDatabase();
        }
        public void declareWinner()
        {
            SqlConnection conn = new SqlConnection(Config.ConnectionString);
            conn.Open();


            String qeury = "select * from Candidate where Votes = (select max(Votes) from Candidate)";

            SqlCommand cmd = new SqlCommand(qeury, conn);


            SqlDataReader dr = cmd.ExecuteReader();

            // using loop ( in case there is a tie )
            while (dr.Read())
            {
                Console.WriteLine("Winner is: " + dr[1]);
            }
        }
        public void insertCandidate(Candidate c)
        {
            addCandidateToDatabase(c); // order is imp for integrity of PK
            addObjectToFile(c, "candidates.txt");
        }
        // the definition of this function is altered a bit 
        // but still the public interface is same due to default argument
        public Candidate readCandidate(String cnic , bool show = true) 
        {
            
            Candidate c;
            c = readCandidateFromFile(cnic , show);
            if (show)
            {
                Console.WriteLine("Candidate from File: ");
                Console.WriteLine();
                Console.WriteLine("Candidate from Database: ");
                readCandidateFromDB(cnic);
            }
            return c;
        }
        public void updateCandidate(String cnic)
        {
            updateCandidateInFile(cnic); // <---- this function is also making call
                                         // to update candidate in database
                                         // coz it was annoying to take as input
                                         // candidate details twice
        }
        public void deleteCandidate(string cnic)
        {
            deleteCandidateFromFile(cnic);
            deleteCandidateFromDB(cnic);
        }
        public void deleteVoter(String cnic)
        {
            deleteVoterFromFile(cnic);
            deleteVoterFromDB(cnic);
        }
    }
}
