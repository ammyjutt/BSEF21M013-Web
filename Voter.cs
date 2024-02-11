using System;
using System.Data.SqlClient;


namespace VotingSystem
{
    internal class Voter
    {

        private String cnic;
        private String voterName;
        private String selectedpartyname;


        public Voter()
        {
            cnic = "";
            voterName = "";
            selectedpartyname = "";
        }

        public String CNIC
        {
            get
            {
                return cnic;
            }
            set
            {
                cnic = value;
            }
        }

        public String VoterName
        {
            get
            {
                return voterName;
            }
            set
            {
                voterName = value;
            }
        }
        

        


        public Voter(string voterName, string cnic, string selectedpartyname)
        {
            this.voterName = voterName;
            this.cnic = cnic;
            this.selectedpartyname = selectedpartyname;
        }

        public String SelectedPartyName
        {
            get
            {
                return selectedpartyname;
            }
            set
            {
                selectedpartyname= value;
            }
        }

        public bool hasVoted(String cnic)
        {
            String connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"MyDB\";Integrated Security=True;";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            String query = "select * from Voter where VoterID = @arg1";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@arg1", cnic);

            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                if (dr[2] == DBNull.Value)
                    return false;
                else 
                    return true;
            }
            return false;
        }



        public override string ToString()
        {
            return $"Voter Name: {voterName,-12} CNIC: {cnic,-12} Selected Party: {SelectedPartyName}";
        }


    }
}
