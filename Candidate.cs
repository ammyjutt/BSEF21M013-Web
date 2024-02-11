using System;

namespace VotingSystem
{
    internal class Candidate
    {

        String candidateId; // this will be cnic of the candidate
        
        private String name;

        private String party;

        private int votes;


        private int GenerateCandidateID()
        {
            return -1; // no need for this - since changed the candidateId to string
        }

        
        public Candidate()
        {
            this.candidateId = "";
            this.name = "";
            this.party = "";
            this.votes = 0;
        }
        public Candidate(String name, String party, string candidateId)
        {
            this.candidateId = candidateId;
            this.name = name;
            this.party = party;
            this.votes = 0;
        }

        public void insertCandidate(Candidate c)
        {
            Console.Write("Enter Name: ");
            this.name = Console.ReadLine();
            Console.Write("Enter Party: ");
            this.party = Console.ReadLine();

            // add candidate to file


        }

        public void castVote()
        {
            votes++; // the functionality of this function was unclear
        }

        public string CandidateID
        {
            get
            {
                return candidateId;
            }
            set
            {
                // we can't do serialization/deserialization without setter
                candidateId = value;
            }
        }
        
        public String Name 
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public String Party
        {
            get
            {
                return party;
            }
            set
            {
                party = value;
            }
        }

        public int Votes
        {
            get
            {
                return votes;
            }
            set
            {
                votes = value;
            }
        }

        public void IncrementVotes()
        {
            votes += 1;
        }

        public override string ToString()
        {
            return $"Candidate ID: {CandidateID,-5} Name: {name,-10}     Party: {party,-10} Votes: {Votes,1}";
        }




    }
}
