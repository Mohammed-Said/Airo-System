
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Xml.Linq;
using System.Reflection;
using Spectre.Console;
using System.Data;

namespace c_Airline
{
  class Flight : ICrudOperation
  {


    private bool isDeleted;





    //Properties 
    public int? FlightID { get; set; }
    public int? AircraftId { get; set; }
    public int? RouteId { get; set; }
    public int? NumOfPassenger { get; set; }
    public int? RemainingSeats { get; set; }
    public DateTime? DepartedTime { get; set; }
    public DateTime? ArrivalTime { get; set; }
    public int? TimeSpent { get; set; }
    public decimal? PricePerPassenger { get; set; }
    public bool? IsDeleted { get; set; }

    public string? Origins { get; set; }
    public string? Dest { get; set; }

    //Main Functions : 
    //Function To create a new Row IN Flight class
    public bool Create()
    {
      try
      {
        bool isCreated = false;
        do
        {
          //To Ensure That ID is Int
          Console.Write("Enter AircraftID: ");

          int InputID;
          while (!int.TryParse(Console.ReadLine(), out InputID) || InputID <= 0)
          {
            NotValidIDPositive();
          }

          //To Ensure That ID Aircraft is in our DB and Then the continue the process 
          if (IsCraeted(ref InputID, ref isCreated)) { }

        } while (!isCreated);


      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error creating Aircraft: {ex.Message}");
        return false;
      }
      return true;
    }


    //To Edit Specific Row With It's ID
    public bool Edit()
    {
      try
      {

        bool isUpdated = false;
        do
        {
          //To Ensure That ID is Int
          Console.Write("Enter FlightId of Row You Want To Edit: ");

          int FlightId;
          while (!int.TryParse(Console.ReadLine(), out FlightId) || FlightId <= 0)
          {
            NotValidIDPositive();
          }

          //To Ensure That ID Aircraft is in our DB and Then the continue the process 
          if (IsUpdated(ref FlightId, ref isUpdated)) { }

        } while (!isUpdated);


      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error updating Aircraft: {ex.Message}");
        return false;
      }
      return true;
    }



    //To Delete Specific Row With It's ID
    public bool Delete()
    {
      try
      {


        bool deletionSuccessful = false;

        do
        {
          //To Ensure That ID is Int
          Console.Write("Enter Flight ID of Row You Want To Delete: ");

          int inputID;
          while (!int.TryParse(Console.ReadLine(), out inputID) || inputID <= 0)
          {
            NotValidIDPositive();
          }

          //To Ensure That ID is in our DB
          if (IsIDDeleted(inputID, ref deletionSuccessful)) { }

        } while (!deletionSuccessful); // Continue until the deletion is successful or the user decides not to try again


      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error deleting Aircraft: {ex.Message}");
        return false;
      }
      return true;
    }


    //To Show all Rows 
    public void GetAll()
    {
      List<Flight> flights = new List<Flight>();
      try
      {
        string query = "SELECT * FROM Flights";
        using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
        {
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              flights.Add(new Flight()
              {
                FlightID = (int?)reader["FlightID"],
                AircraftId = reader.IsDBNull(reader.GetOrdinal("AircraftID")) ? null : (int?)reader["AircraftID"],
                RouteId = reader.IsDBNull(reader.GetOrdinal("Route_ID")) ? null : (int?)reader["Route_ID"],
                DepartedTime = reader.IsDBNull(reader.GetOrdinal("Dept_Date"))?null:(DateTime?)reader["Dept_Date"],
                NumOfPassenger = reader.IsDBNull(reader.GetOrdinal("Num_OF_Passanger")) ? null : (int?)reader["Num_OF_Passanger"],
                PricePerPassenger = reader.IsDBNull(reader.GetOrdinal("Price"))?null: (decimal?)reader["Price"],
                ArrivalTime = reader.IsDBNull(reader.GetOrdinal("Arrival_Date")) ? null : (DateTime?)reader["Arrival_Date"],     
                TimeSpent = reader.IsDBNull(reader.GetOrdinal("Time_Spent")) ? null:(int?)reader["Time_Spent"],
                IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : (bool)reader["IsDeleted"]
              });
            }
          }
        }
        Console.WriteLine("All Flights:");
        TableMaker.PrintLine();
        TableMaker.PrintRow("FlightID", "AircraftID", "Route_ID", "Num OF Passanger", "Price", "Dept_Date", "Arrival_Date", "Time_Spent", "IsDeleted");
        TableMaker.PrintLine();
        foreach (var a in flights)
        {
          //Console.WriteLine($"FlightID: {a.FlightID}, AircraftID: {a.AircraftId}, Route_ID: {a.RouteId}," +
          //    $" Num Of Passengers: {a.NumOfPassenger},Price Per Passenger: {a.PricePerPassenger}, Departed Time: {a.DepartedTime}, ArrivalTime: {a.ArrivalTime}," +
          //    $"TimeSpent: {a.TimeSpent} , IsDeleted: {a.IsDeleted}");
          TableMaker.PrintRow(a.FlightID.ToString(), a.AircraftId.ToString(), a.RouteId.ToString(), a.NumOfPassenger.ToString(), a.PricePerPassenger.ToString(),
            a.DepartedTime.ToString(), a.ArrivalTime.ToString(), a.TimeSpent.ToString(), a.IsDeleted.ToString());
          TableMaker.PrintLine();
        }


        //Successfully Statement

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("The Rows of Flight are Getted Successfully.");
        Console.ForegroundColor = ConsoleColor.White;

      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error retrieving Aircraft: {ex.Message}");
      }
    }


    //To Get Specific Row With It's ID
    public void GetByID()
    {
      try
      {

        bool deletionSuccessful = false;

        do
        {
          //To Ensure That ID is Int
          Console.Write("Enter Flight ID of Row You Want To Get: ");

          int inputID;
          while (!int.TryParse(Console.ReadLine(), out inputID) || inputID <= 0)
          {
            NotValidIDPositive();
          }

          //To Ensure That ID is in our DB
          if (IsGetted(inputID, ref deletionSuccessful)) { }

        } while (!deletionSuccessful); // Continue until the deletion is successful or the user decides not to try again



      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error retrieving Aircraft: {ex.Message}");
      }
    }






    //Helper Functions :

    //The NotValidIDPositive Message
    private void NotValidIDPositive()
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Write("Invalid input. Please enter a positive integer: ");
      Console.ForegroundColor = ConsoleColor.White;
    }

    //The NotValidIDPositive Message
    private void NotValidIDDecimal()
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Write("Invalid input . Please enter a positive Decimal: ");
      Console.ForegroundColor = ConsoleColor.White;
    }


    //The NotValid of AirlineID Existance Message
    private void NotValidIDExistance()
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine("Invalid AirlineID. The specified AirlineID does not Exist.");
      Console.ForegroundColor = ConsoleColor.White;

    }

    //The NotValid of Route ID Existance Message
    private void NotValidIDRouteExistance()
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine("Invalid Route ID. The specified Route ID does not Exist.");
      Console.ForegroundColor = ConsoleColor.White;

    }


    //The NotValid of Flight ID Existance Message
    private void NotValidIDFlightExistance()
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine("Invalid Flight ID. The specified Flight ID does not Exist.");
      Console.ForegroundColor = ConsoleColor.White;

    }

    //The NotValidDate Message
    private void NotValidDate()
    {
      Console.ForegroundColor = ConsoleColor.Red;

      Console.WriteLine("Invalid date format. Please enter the date in the format (yyyy-MM-dd HH:mm:ss).");
      Console.Write("Enter Date again: ");

      Console.ForegroundColor = ConsoleColor.White;
    }



    //
    //

    //Function that check if AirlineID is on DB
    private bool IsFlightIDExist(int FlightID)
    {
      bool exists = false;

      try
      {
        string query = "IF EXISTS (SELECT 1 FROM Flights WHERE FlightID = @FlightID)SELECT 1 ELSE SELECT 0";

        using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
        {
          command.Parameters.AddWithValue("@FlightID", FlightID);
          exists = ((int)command.ExecuteScalar() == 1);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error checking FlightID existence: {ex.Message}");
      }

      return exists;
    }


    //Function that check if AirlineID is on DB
    private bool IsIDExist(int AirlineID)
    {
      bool exists = false;

      try
      {
        string query = "IF EXISTS (SELECT 1 FROM Airline WHERE AirlineID = @AirlineID)SELECT 1 ELSE SELECT 0";

        using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
        {
          command.Parameters.AddWithValue("@AirlineID", AirlineID);
          exists = ((int)command.ExecuteScalar() == 1);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error checking AirlineID existence: {ex.Message}");
      }

      return exists;
    }


    //Function that check if routeId is on DB
    private bool IsIDRouteExist(int routeId)
    {
      bool exists = false;

      try
      {
        string query = "IF EXISTS (SELECT 1 FROM Route WHERE Route_ID = @routeId)SELECT 1 ELSE SELECT 0";


        using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
        {
          command.Parameters.AddWithValue("@routeId", routeId);
          exists = ((int)command.ExecuteScalar() == 1);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error checking routeId existence: {ex.Message}");
      }

      return exists;
    }




    //Create _________________________

    //Function That checked if airlineId is IsCraeted correctly 
    private bool IsRowCreated(ref int routeId, ref bool isIDAirUpdated, ref int AirId)
    {

      if (IsIDRouteExist(routeId))
      {

        Console.Write("Enter num Of Passengers: ");

        int numOfPassengers;
        while (!int.TryParse(Console.ReadLine(), out numOfPassengers) || numOfPassengers <= 0)
        {
          NotValidIDPositive();
        }

        Console.Write("Enter Price per Passenger: ");

        decimal pricePerPassenger;
        while (!decimal.TryParse(Console.ReadLine(), out pricePerPassenger) || pricePerPassenger <= 0)
        {
          NotValidIDDecimal();

        }

        Console.Write("Enter Departed Time (YYYY-MM-DD HH:mm:ss): ");
        DateTime departedTime;

        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out departedTime))
        {
          NotValidDate();
        }

        Console.Write("Enter Arrival Time (yyyy-MM-dd HH:mm:ss): ");
        DateTime arrivalTime;
        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out arrivalTime))
        {
          NotValidDate();
        }

        isDeleted = true;

        string query = "INSERT INTO flights (AircraftID, Route_ID, Dept_Date, Num_OF_Passanger, Price, Arrival_Date, IsDeleted) VALUES (@newId, @routeId, @deptDate, @numOfPassengers, @pricePerPassenger, @ArrivalDate, @IsDeleted)";
        using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
        {
          command.Parameters.AddWithValue("@newId", AirId);
          command.Parameters.AddWithValue("@routeId", routeId);
          command.Parameters.AddWithValue("@numOfPassengers", numOfPassengers);
          command.Parameters.AddWithValue("@pricePerPassenger", pricePerPassenger);
          command.Parameters.AddWithValue("@deptDate", departedTime);
          command.Parameters.AddWithValue("@ArrivalDate", arrivalTime);
          command.Parameters.AddWithValue("@IsDeleted", isDeleted);
          command.ExecuteNonQuery();
        }


        isIDAirUpdated = true;

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("The Row of Flights Is Created Successfully.");
        Console.ForegroundColor = ConsoleColor.White;

        return isIDAirUpdated;
      }


      else
      {
        NotValidIDRouteExistance();
        return false;
      }
    }


    //Function That checked if Row is IsCraeted Main One 
    private bool IsCraeted(ref int inputID, ref bool isUpdated)
    {

      if (IsIDExist(inputID))
      {
        bool isIDAirUpdated = false;
        do
        {

          Console.Write("Enter RouteID: ");
          int routeId;
          while (!int.TryParse(Console.ReadLine(), out routeId))
          {
            NotValidIDPositive();

          }

          //To Ensure That ID _AirlineID is in our DB and create That spcific Row
          if (IsRowCreated(ref routeId, ref isIDAirUpdated, ref inputID)) { }

          isUpdated = true;


        } while (!isIDAirUpdated);
      }
      else
      {
        NotValidIDExistance();
        return false;
      }

      return isUpdated;

    }





    //Update _________________________


    //Function That checked if Route is Updated correctly  and the process of updating
    private bool IsRouteUpdated(ref int FlightID, ref bool isIDRouteUpdated, ref int AirId, ref int routeId)
    {

      if (IsIDRouteExist(routeId))
      {

        Console.Write("Enter num Of Passengers: ");

        int numOfPassengers;
        while (!int.TryParse(Console.ReadLine(), out numOfPassengers) || numOfPassengers <= 0)
        {
          NotValidIDPositive();
        }

        Console.Write("Enter Price per Passenger: ");

        decimal pricePerPassenger;
        while (!decimal.TryParse(Console.ReadLine(), out pricePerPassenger) || pricePerPassenger <= 0)
        {
          NotValidIDDecimal();

        }

        Console.Write("Enter Departed Time yyyy-MM-dd HH:mm:ss : ");
        DateTime departedTime;

        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out departedTime))
        {
          NotValidDate();
        }

        Console.Write("Enter Arrival Time yyyy-MM-dd HH:mm:ss : ");
        DateTime arrivalTime;
        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out arrivalTime))
        {
          NotValidDate();
        }

        isDeleted = true;

        string query = "UPDATE flights SET AircraftID=@newId,Route_ID=@routeId,Dept_Date=@deptDate,Num_OF_Passanger=@numOfPassengers,Price=@pricePerPassenger,Arrival_Date=@ArrivalDate,IsDeleted= @IsDeleted WHERE FlightID=@InputID";
        using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
        {
          command.Parameters.AddWithValue("@newId", AirId);
          command.Parameters.AddWithValue("@routeId", routeId);
          command.Parameters.AddWithValue("@numOfPassengers", numOfPassengers);
          command.Parameters.AddWithValue("@pricePerPassenger", pricePerPassenger);
          command.Parameters.AddWithValue("@deptDate", departedTime);
          command.Parameters.AddWithValue("@ArrivalDate", arrivalTime);
          command.Parameters.AddWithValue("@IsDeleted", isDeleted);
          command.Parameters.AddWithValue("@InputID", FlightID);
          command.ExecuteNonQuery();
        }



        isIDRouteUpdated = true;

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("The Row of Flights Is Updated Successfully.");
        Console.ForegroundColor = ConsoleColor.White;

        return isIDRouteUpdated;
      }


      else
      {
        NotValidIDRouteExistance();
        return false;
      }
    }


    //Function That checked if airlineId is Updated correctly 
    private bool IsRowUpdated(ref int FlightID, ref bool isIDAirUpdated, ref int AirId)
    {

      if (IsIDExist(AirId))
      {
        bool isIDRouteUpdated = false;
        do
        {

          Console.Write("Enter RouteID: ");

          int routeId;
          while (!int.TryParse(Console.ReadLine(), out routeId))
          {
            NotValidIDPositive();

          }



          //To Ensure That ID _AirlineID is in our DB and create That spcific Row
          if (IsRouteUpdated(ref FlightID, ref isIDRouteUpdated, ref AirId, ref routeId)) { }

          isIDAirUpdated = true;


        } while (!isIDRouteUpdated);

      }

      else
      {
        NotValidIDExistance();
        return false;
      }
      return isIDAirUpdated;

    }



    //Function That checked if Row is Updated Main One 
    private bool IsUpdated(ref int FlightID, ref bool isUpdated)
    {

      if (IsFlightIDExist(FlightID))
      {
        bool isIDAirUpdated = false;
        do
        {

          Console.Write("Enter AircraftID: ");
          int AircraftID;
          while (!int.TryParse(Console.ReadLine(), out AircraftID))
          {
            NotValidIDPositive();

          }


          //To Ensure That ID _AirlineID is in our DB and create That spcific Row
          if (IsRowUpdated(ref FlightID, ref isIDAirUpdated, ref AircraftID)) { }

          isUpdated = true;


        } while (!isIDAirUpdated);
      }
      else
      {
        NotValidIDFlightExistance();
        return false;
      }

      return isUpdated;

    }



    //Delete_____________________
    //Function That checked if Row is Deleted 
    private bool IsIDDeleted(int inputID, ref bool deletionSuccessful)
    {

      if (IsFlightIDExist(inputID))
      {

        string query = "DELETE FROM Flights WHERE FlightID = @InputID";
        using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
        {
          command.Parameters.AddWithValue("@InputID", inputID);
          command.ExecuteNonQuery();
        }

        deletionSuccessful = true;

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("The Row of Flight Deleted Successfully.");
        Console.ForegroundColor = ConsoleColor.White;

        return deletionSuccessful;
      }
      else
      {
        NotValidIDFlightExistance();
        return false;
      }
    }





    //GetById ____________________
    //Function That checked if Row is getten 
    private bool IsGetted(int inputID, ref bool IsGetting)
    {

      if (IsFlightIDExist(inputID))
      {

        string query = "SELECT * FROM Flights WHERE FlightID = @InputID";

        using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
        {
          command.Parameters.AddWithValue("@InputID", inputID);
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              FlightID = (int?)reader["FlightID"];
              AircraftId = reader.IsDBNull(reader.GetOrdinal("AircraftID")) ? null : (int?)reader["AircraftID"];
                RouteId = reader.IsDBNull(reader.GetOrdinal("Route_ID")) ? null : (int?)reader["Route_ID"];
              DepartedTime = reader.IsDBNull(reader.GetOrdinal("Dept_Date")) ? null : (DateTime?)reader["Dept_Date"];
              NumOfPassenger = reader.IsDBNull(reader.GetOrdinal("Num_OF_Passanger")) ? null : (int?)reader["Num_OF_Passanger"];
              PricePerPassenger = reader.IsDBNull(reader.GetOrdinal("Price")) ? null : (decimal?)reader["Price"];
                ArrivalTime = reader.IsDBNull(reader.GetOrdinal("Arrival_Date")) ? null : (DateTime?)reader["Arrival_Date"] ;
              TimeSpent = reader.IsDBNull(reader.GetOrdinal("Time_Spent")) ? null : (int?)reader["Time_Spent"];
              IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : (bool)reader["IsDeleted"];
            }
          }
        }
        TableMaker.PrintLine();
        TableMaker.PrintRow("FlightID", "AircraftID", "Route_ID", "Num OF Passanger", "Price", "Dept_Date", "Arrival_Date", "Time_Spent", "IsDeleted");
        TableMaker.PrintLine();
        TableMaker.PrintRow(FlightID.ToString(), AircraftId.ToString(), RouteId.ToString(), NumOfPassenger.ToString(), PricePerPassenger.ToString(),
            DepartedTime.ToString(), ArrivalTime.ToString(), TimeSpent.ToString(), IsDeleted.ToString());
        TableMaker.PrintLine();
        //Console.WriteLine($"FlightID: {FlightID}, AircraftID: {AircraftId}, Route_ID: {RouteId}," +
        //    $" Num Of Passengers: {NumOfPassenger},Price Per Passenger: {PricePerPassenger}, Departed Time: {DepartedTime}, ArrivalTime: {ArrivalTime}," +
        //    $"TimeSpent: {TimeSpent} , IsDeleted: {IsDeleted}");


        IsGetting = true;

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("The Row of Flight Is Getted Successfully.");
        Console.ForegroundColor = ConsoleColor.White;


        return IsGetting;
      }
      else
      {
        NotValidIDFlightExistance();
        return false;
      }
    }


    
  }
  #region Future Functions :
  //public int TotalPrice()
  //{
  //    return 1;
  //}

  //public string OriginCity()
  //{
  //    return "";
  //}

  //public string DestinationCity()
  //{
  //    return "";
  //}

  //public void RouteInfo()
  //{

  //}

  //public void FlightInfo()
  //{

  //}
  #endregion


}
