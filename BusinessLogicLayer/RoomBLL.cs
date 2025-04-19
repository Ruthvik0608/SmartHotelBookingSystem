using SmartHotelBookingSystem.DataAccess.ADO;
using SmartHotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SmartHotelBookingSystem.BusinessLogicLayer
{
    public class RoomBLL
    {
        private readonly DB1 _dalObject;

        public RoomBLL(DB1 dalObject)
        {
            _dalObject = dalObject;
        }

        #region RoomTable

        public int InsertRoom(Room_New room)
        {
            string insertQuery = @"INSERT INTO [dbo].[Room_New]
                                    (
                                    [HotelID]
                                    ,[Type]
                                    ,[Price]
                                    ,[Availability]
                                    ,[Features]
                                    ,[IsActive])
                                    VALUES                                     
                                    (
                                    @HotelID
                                    ,@Type
                                    ,@Price
                                    ,@Availability
                                    ,@Features
                                    ,@IsActive)";

            nameValuePairList nvp = new nameValuePairList();
            nvp.Add(new nameValuePair("@HotelID", room.HotelID));
            nvp.Add(new nameValuePair("@Type", room.Type));
            nvp.Add(new nameValuePair("@Price", room.Price));
            nvp.Add(new nameValuePair("@Availability", room.Availability));
            nvp.Add(new nameValuePair("@Features", room.Features));
            nvp.Add(new nameValuePair("@IsActive", room.IsActive));

            int insertStatus = 0;

            try
            {
                insertStatus = _dalObject.InsertUpdateOrDelete(insertQuery, nvp, false);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error: " + exp.Message);
            }

            return insertStatus;
        }

        public int UpdateRoom(Room_New room)
        {
            string updateQuery = @"UPDATE [dbo].[Room_New]
                            SET
                            [HotelID]=@HotelID,
                            [Type] = @Type
                            ,[Price] = @Price
                            ,[Availability] = @Availability
                            ,[Features] = @Features
                            ,[IsActive] = @IsActive
                            WHERE [RoomID] = @RoomID";

            nameValuePairList nvp = new nameValuePairList();
            nvp.Add(new nameValuePair("@HotelID", room.HotelID));
            nvp.Add(new nameValuePair("@Type", room.Type));
            nvp.Add(new nameValuePair("@Price", room.Price));
            nvp.Add(new nameValuePair("@Availability", room.Availability));
            nvp.Add(new nameValuePair("@Features", room.Features));
            nvp.Add(new nameValuePair("@IsActive", room.IsActive));
            nvp.Add(new nameValuePair("@RoomID", room.RoomID));

            int updateStatus = 0;

            try
            {
                updateStatus = _dalObject.InsertUpdateOrDelete(updateQuery, nvp, false);
                Console.WriteLine("Update Status: " + updateStatus);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error: " + exp.Message);
            }

            return updateStatus;
        }


        public DataTable DeleteRoom(Room_New room)
        {
            string deleteRoomQuery = @"UPDATE [dbo].[Room_New]
                        SET [IsActive] = 0
                        WHERE [RoomID] = @RoomID";

            nameValuePairList nvp = new nameValuePairList();
            nvp.Add(new nameValuePair("@RoomID", room.RoomID));

            int deleteStatus = 0;

            try
            {
                deleteStatus = _dalObject.InsertUpdateOrDelete(deleteRoomQuery, nvp, false);
                Console.WriteLine("Delete Status: " + deleteStatus);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error: " + exp.Message);
            }

            if (deleteStatus > 0)
            {
                string fetchRoomsQuery = @"SELECT
                             [RoomID],
                             [HotelID],
                             [Type],
                             [Price],
                             [Availability],
                             [Features],
                             [IsActive]
                            FROM [dbo].[Room_New]
                            WHERE [IsActive] = 1";

                DataTable dt = _dalObject.FetchData(fetchRoomsQuery);
                Console.WriteLine("Fetched Rows: " + (dt != null ? dt.Rows.Count : 0));
                return dt;
            }
            else
            {
                return null;
            }
        }


        //public DataTable SearchRoom(string type)
        //{
        //    string searchRoomQuery = @"SELECT [RoomID],
        //                                   [HotelID],
        //                                   [Type],
        //                                   [Price],
        //                                   [Availability],
        //                                   [Features],
        //                                   [IsActive]
        //                       FROM [dbo].[Room_New] 
        //                       WHERE [Type] LIKE '%' + @Type + '%'";

        //    nameValuePairList nvp = new nameValuePairList();
        //    nvp.Add(new nameValuePair("@Type", type));

        //    DataTable dt = _dalObject.FillAndReturnDataTable(searchRoomQuery, nvp);
        //    return dt;
        //}

        //public DataTable UpdateFeatures(Room_New room)
        //{
        //    string updateFeaturesQuery = @"UPDATE [dbo].[Room_New]
        //                            SET [Features] = @Features
        //                            WHERE [RoomID] = @RoomID and IsActive = 1";

        //    nameValuePairList nvp = new nameValuePairList();
        //    nvp.Add(new nameValuePair("@Features", room.Features));
        //    nvp.Add(new nameValuePair("@RoomID", room.RoomID));

        //    DataTable dt = _dalObject.FillAndReturnDataTable(updateFeaturesQuery, nvp);
        //    return dt;
        //}

        public DataTable FetchAllActiveRooms()
        {
            string fetchAllActiveRoomsQuery = @"SELECT
                                                 [RoomID],
                                                 [HotelID],
                                                 [Type],
                                                 [Price],
                                                 [Availability],
                                                 [Features],
                                                 [IsActive]
                                                FROM [dbo].[Room_New]
                                                WHERE [IsActive] = 1";

            DataTable dt = null;

            try
            {
                dt = _dalObject.FetchData(fetchAllActiveRoomsQuery);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error: " + exp.Message);
            }

            return dt;
        }
        #endregion


        public DataTable FetchRoomsByHotel(int hotelID)
        {
            string fetchRoomsQuery = @"SELECT RoomID, IsActive
                                        FROM Room_New
                                        WHERE HotelID = @HotelID AND IsActive = 1";

            nameValuePairList nvp = new nameValuePairList();
            nvp.Add(new nameValuePair("@HotelID", hotelID));

            DataTable dt = null;

            try
            {
                dt = _dalObject.FillAndReturnDataTable(fetchRoomsQuery, nvp);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error: " + exp.Message);
            }

            return dt;
        }
        public DataTable FetchRoomsByTypeAndLocation(string type, int hotelID)
        {
            string fetchRoomsByTypeAndLocationQuery = @"SELECT
                                                 r.[RoomID],
                                                 r.[HotelID],
                                                 r.[Type],
                                                 r.[Price],
                                                 r.[Availability],
                                                 r.[Features],
                                                 r.[IsActive],
                                                 h.[Name] AS HotelName,
                                                 h.[Location]
                                                FROM [dbo].[Room_New] r
                                                INNER JOIN [dbo].[Hotels] h ON r.[HotelID] = h.[HotelID]
                                                WHERE r.[Type] = @Type AND r.[HotelID] = @HotelID AND r.[IsActive] = 1";

            nameValuePairList nvp = new nameValuePairList();
            nvp.Add(new nameValuePair("@Type", type));
            nvp.Add(new nameValuePair("@HotelID", hotelID));

            DataTable dt = null;

            try
            {
                Console.WriteLine($"Executing query with Type: {type}, HotelID: {hotelID}");
                dt = _dalObject.FillAndReturnDataTable(fetchRoomsByTypeAndLocationQuery, nvp);
                Console.WriteLine($"Fetched Rows for Type {type} and HotelID {hotelID}: " + (dt != null ? dt.Rows.Count : 0));
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error: " + exp.Message);
            }

            return dt;
        }



        public List<Room_New> ConvertDataTableToList(DataTable dataTable)
        {
            var roomList = new List<Room_New>();

            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    var room = new Room_New
                    {
                        RoomID = row.Field<int?>("RoomID") ?? 0,
                        HotelID = row.Field<int?>("HotelID") ?? 0,
                        Type = row.Field<string>("Type") ?? string.Empty,
                        Price = (float)(row.Field<double?>("Price") ?? 0),
                        Availability = row.Field<string>("Availability") ?? string.Empty,
                        Features = row.Field<string>("Features") ?? string.Empty,
                        IsActive = row.Field<bool?>("IsActive") ?? false
                    };

                    roomList.Add(room);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error converting DataRow to Room_New: {ex.Message}", ex);
                }
            }

            return roomList;
        }

    }
}





