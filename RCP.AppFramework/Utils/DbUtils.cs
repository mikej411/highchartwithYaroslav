using Browser.Core.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RCP.AppFramework;

namespace RCP.AppFramework
{
    public static class DbUtils
    {
        //private static readonly DataAccess _WebAppDbAccess = new DataAccess(new SqlServerDataAccessProvider(Constants.SQLconnString));
        private static readonly DataAccess _WebAppDbAccess = new DataAccess(new MySqlDataAccessProvider(Constants.SQLconnString));

        #region Queries


        /// <summary>
        /// Returns a list in the database 
        /// </summary>
        public static List<string> GetPersonsToListString()
        {
            List<string> listItems = new List<string>();

            int count = _WebAppDbAccess.GetDataValue<int>(@"SELECT count(Name) 
																		 FROM ");
            for (var i = 1; i <= count; i++)
            {
                var sqlstring = string.Format(@"SELECT Name FROM (
												SELECT ROW_NUMBER() OVER (ORDER BY Age ASC) AS RowNumber, *
												FROM ADO.Persons) AS foo
												WHERE RowNumber = {0}", i);
                listItems.Add(_WebAppDbAccess.GetDataValue(sqlstring).ToString());
            }

            return listItems;
        }

        /// <summary>
        /// Returns a list of People
        /// </summary>
        public static List<string> GetFilteredNetworksToListString(string likeCondition)
        {
            List<string> listItems = new List<string>();
            int count = _WebAppDbAccess.GetDataValue<int>(@"SELECT count(Name) 
																		 FROM ADO.Persons");
            for (var i = 1; i <= count; i++)
            {
                var sqlstring = string.Format(@"SELECT Name
                                                FROM ADO.Persons ", i);
                listItems.Add(_WebAppDbAccess.GetDataValue(sqlstring).ToString());
            }
            return listItems;
        }

        public static DataTable GetDataTableOfSomething()
        {
            var results = _WebAppDbAccess.GetDataTable(
                @"");
            return results;
        }

        /// <summary>
        /// Returns a list of People
        /// </summary>
        public static List<string> GetGroupLearningActivities()
        {
            List<string> listItems = new List<string>();
            int count = _WebAppDbAccess.GetDataValue<int>(@"SELECT count(Name) 
																		 FROM ADO.Persons");
            for (var i = 1; i <= count; i++)
            {
                var sqlstring = string.Format(@"SELECT Name
                                                FROM ADO.Persons ", i);
                listItems.Add(_WebAppDbAccess.GetDataValue(sqlstring).ToString());
            }
            return listItems;
        }

        /// <summary>
        /// Inserts a record into the person table
        /// </summary>
        /// <param name="personGuid"></param>
        /// <param name="name"></param>
        /// <param name="age"></param>
        public static void InsertPerson(string personGuid, string name, string age)
        {
            var sqlstring = string.Format(@"INSERT INTO mjschema.person  (idperson, Name, Age) VALUES({0}, '{1}', {2});", personGuid, name, age);

            _WebAppDbAccess.ExecuteNonQuery(sqlstring);
        }

        /// <summary>
        /// Inserts a record into the person table
        /// </summary>
        /// <param name="personAddressGuid"></param>
        /// <param name="Address"></param>
        public static void InsertPersonAddress(string personAddressGuid, string Address)
        {

            var sqlstring = string.Format(@"insert into 
 values 
 ('{0}','{1}');", personAddressGuid, Address);

            _WebAppDbAccess.ExecuteNonQuery(sqlstring);
        }

        /// <summary>
        /// Deletes a record from the Person table
        /// <param name="personGuid">The Guid of the map record you want to delete</param>
        /// </summary>
        public static void DeletePerson(string personGuid)
        {

            var sqlstring = string.Format(@"DELETE FROM WHERE ='{0}'; ", personGuid);

            _WebAppDbAccess.ExecuteNonQuery(sqlstring, 10000);
        }

        /// <summary>
        /// Deletes a record from the PersonAddress table
        /// <param name="personGuid">The Guid of the map record you want to delete</param>
        /// </summary>
        public static void DeletePersonAddress(string personAddressGuid)
        {

            var sqlstring = string.Format(@"DELETE FROM WHERE ='{0}'; ", personAddressGuid);

            _WebAppDbAccess.ExecuteNonQuery(sqlstring, 10000);
        }

        /// <summary>
        /// Grabs a record from the Person table
        /// <param name="Guid"></param>
        /// </summary>
        public static DataRow GetPersonRecordByGuid(string Guid)
        {
            string sqlString = string.Format(@"select * from  
WHERE = '{0}'", Guid);

            return _WebAppDbAccess.GetDataRow(sqlString);
        }

        public static string GetGuidOfPersonRecord(string personName)
        {
            string sqlString = string.Format(@"select from 
where  = ''", personName);

            return _WebAppDbAccess.GetDataValue<string>(sqlString);
        }

        /// <summary>
        /// Deletes all records that were created from InsertPerson. 
        /// <param name="recordSet">The record set created from InsertPerson of type PersonRecord</param>
        /// </summary>
        public static void DeletePersonRecordSets(PersonRecord recordSet)
        {
            if (recordSet.PersonGuid != "")
            {
                DbUtils.DeletePerson(recordSet.PersonGuid);
            }

            if (recordSet.AddressGuid != "")
            {
                DbUtils.DeletePersonAddress(recordSet.AddressGuid);
            }
        }

        #endregion Queries


    }
}


public class PersonRecord
{
    public string PersonGuid { get; set; }
    public string Name { get; set; }
    public string Age { get; set; }
    public string AddressGuid { get; set; }
    public string Address { get; set; }

    public PersonRecord(string personGuid, string name, string age, string addressGuid, string address)
    {
        PersonGuid = PersonGuid;
        Name = name;
        Age = age;
        AddressGuid = addressGuid;
        Address = address;
    }
}



