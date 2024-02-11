using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;




///                                                   MUHAMMAD AMIR
///                                                   BSEF21 M013



namespace VotingSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {

                //                                          some preprocessing


                // Create empty ones if they don't exist to avoid errors

                if(!File.Exists("candidates.txt"))
                {
                    File.Create("candidates.txt").Dispose();
                }
                if (!File.Exists("voter.txt"))
                {
                    File.Create("voter.txt").Dispose();
                }


                //     Simulate Voting

                VotingMachine vm = new VotingMachine();

                // Main Menu

                while (true)
                {
                    Console.WriteLine("Press Any Key.. ");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("Menu:");
                    Console.WriteLine("1. Add Voter");
                    Console.WriteLine("2. Update Voter");
                    Console.WriteLine("3. Delete Voter");
                    Console.WriteLine("4. Display Voters");
                    Console.WriteLine("5. Cast Vote");
                    Console.WriteLine("6. Insert Candidate");
                    Console.WriteLine("7. Update Candidate");
                    Console.WriteLine("8. Display Candidates");
                    Console.WriteLine("9. Delete Candidates");
                    Console.WriteLine("10. Declare Winner");
                    Console.WriteLine("0. Exit");

                    Console.WriteLine("Enter your choice: ");
                    int choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            vm.addVoter();
                            break;
                        case 2:
                            Console.WriteLine("Enter CNIC of the voter to update: ");
                            string cnicToUpdate = Console.ReadLine();
                            vm.updateVoter(cnicToUpdate);
                            break;
                        case 3:
                            Console.WriteLine("Enter CNIC of the voter to delete: ");
                            string cnicToDelete = Console.ReadLine();
                            vm.deleteVoter(cnicToDelete);
                            break;
                        case 4:
                            vm.displayVoters();
                            break;
                        case 5:
                            Console.WriteLine("Enter CNIC of the voter: ");
                            String cnicToVote = Console.ReadLine();
                            Console.WriteLine("Enter CNIC of the candidate: ");
                            String cnicOfCandidate = Console.ReadLine();
                            Candidate candidate = vm.readCandidate(cnicOfCandidate, show: false);
                            // i would have been better if method CastVote() received the cnic 
                            // instead Candidate and Voter object so that I don't had to make these
                            // dummy objects for passing as arg to CastVote()
                            Voter voter = new Voter("", cnicToVote, "");
                            vm.castVote(candidate, voter);
                            break;
                        case 6:

                            String name, id, party;
                            Console.WriteLine("Enter Name: ");
                            name = Console.ReadLine();
                            Console.WriteLine("Enter Party: ");
                            party = Console.ReadLine();
                            Console.WriteLine("Enter CNIC: ");
                            id = Console.ReadLine();
                            Candidate c = new Candidate(name, party, id);
                            vm.insertCandidate(c);
                            break;
                        case 7:
                            Console.WriteLine("Enter CNIC of the candidate to update: ");
                            string cnic = Console.ReadLine();
                            vm.updateCandidate(cnic);
                            break;
                        case 8:
                            vm.displayCandidates();
                            break;
                        case 9:
                            Console.WriteLine("Enter CNIC of the candidate to delete: ");
                            string cnicToDeleteCandidate = Console.ReadLine();
                            vm.deleteCandidate(cnicToDeleteCandidate);
                            break;
                        case 10:
                            vm.declareWinner();
                            break;
                        case 0:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
