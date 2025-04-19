using SmartHotelBookingSystem.DataAccess.ADO;
using SmartHotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SmartHotelBookingSystem.BusinessLogicLayer
{
    public class HotelBLL
    {
        private readonly DB1 _dalObject;

        public HotelBLL(DB1 dalObject)
        {
            _dalObject = dalObject;
        }

        public int InsertHotel(Hotel hotel)
        {
            string insertQuery = @"INSERT INTO [SmartHotelDB].[dbo].[Hotel]
                                            ([HotelID], [Name], [Location], [ManagerID], [Amenities], [Rating], [IsActive])
                                            VALUES (@HotelID, @Name, @Location, @ManagerID, @Amenities, @Rating, 1)";
            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@HotelID", hotel.HotelID),
                new nameValuePair("@Name", hotel.Name),
                new nameValuePair("@Location", hotel.Location),
                new nameValuePair("@ManagerID", hotel.ManagerID),
                new nameValuePair("@Amenities", hotel.Amenities),
                new nameValuePair("@Rating", hotel.Rating)
            };
            int insertStatus = _dalObject.InsertUpdateOrDelete(insertQuery, nvp, false);
            return insertStatus;
        }

        public int UpdateHotel(Hotel hotel, int id)
        {
            string updateQuery = @"UPDATE [SmartHotelDB].[dbo].[Hotel]
                                   SET [Name] = @Name, [Location] = @Location, [ManagerID] = @ManagerID,
                                       [Amenities] = @Amenities, [Rating] = @Rating, [IsActive] = @IsActive
                                   WHERE [HotelID] = @Id";
            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@Id", id),
                new nameValuePair("@Name", hotel.Name),
                new nameValuePair("@Location", hotel.Location),
                new nameValuePair("@ManagerID", hotel.ManagerID),
                new nameValuePair("@Amenities", hotel.Amenities),
                new nameValuePair("@Rating", hotel.Rating),
                new nameValuePair("@IsActive", hotel.IsActive)
            };
            int updateStatus = _dalObject.InsertUpdateOrDelete(updateQuery, nvp, false);
            return updateStatus;
        }

        public DataTable DeleteHotel(int id)
        {
            string deleteHotelQuery = @"UPDATE [SmartHotelDB].[dbo].[Hotel]
                                        SET [IsActive] = 0
                                        WHERE [HotelID] = @HotelID";
            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@HotelID", id)
            };
            int deleteStatus = _dalObject.InsertUpdateOrDelete(deleteHotelQuery, nvp, false);
            if (deleteStatus > 0)
            {
                string fetchHotelsQuery = @"SELECT [Name] FROM [SmartHotelDB].[dbo].[Hotel] WHERE [IsActive] = 1";
                DataTable dt = _dalObject.FetchData(fetchHotelsQuery);
                return dt;
            }
            else
            {
                return null;
            }
        }

        public int UpdateHotelAmenities(int hotelId, string amenities)
        {
            string updateAmenitiesQuery = @"UPDATE [SmartHotelDB].[dbo].[Hotel]
                                            SET [Amenities] = @Amenities
                                            WHERE [HotelID] = @HotelID AND [IsActive] = 1";
            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@HotelID", hotelId),
                new nameValuePair("@Amenities", amenities)
            };
            int updateStatus = _dalObject.InsertUpdateOrDelete(updateAmenitiesQuery, nvp, false);
            return updateStatus;
        }

        public int UpdateHotelRating(int hotelId, double rating)
        {
            string updateRatingQuery = @"UPDATE [SmartHotelDB].[dbo].[Hotel]
                                         SET [Rating] = @Rating
                                         WHERE [HotelID] = @HotelID AND [IsActive] = 1";
            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@HotelID", hotelId),
                new nameValuePair("@Rating", rating)
            };
            int updateStatus = _dalObject.InsertUpdateOrDelete(updateRatingQuery, nvp, false);
            return updateStatus;
        }

        public DataTable ReadHotelByManagerId(int managerId)
        {
            string readHotelByManagerQuery = @"SELECT * FROM [SmartHotelDB].[dbo].[Hotel]
                                               WHERE [ManagerID] = @ManagerID AND [IsActive] = 1";
            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@ManagerID", managerId)
            };
            DataTable dt = _dalObject.FetchData(readHotelByManagerQuery, nvp);
            return dt;
        }

        public DataTable FilterHotelsByRating(double minRating, double maxRating)
        {
            string filterHotelsByRatingQuery = @"SELECT * FROM [SmartHotelDB].[dbo].[Hotel]
                                                 WHERE [Rating] BETWEEN @MinRating AND @MaxRating AND [IsActive] = 1";
            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@MinRating", minRating),
                new nameValuePair("@MaxRating", maxRating)
            };
            DataTable dt = _dalObject.FetchData(filterHotelsByRatingQuery, nvp);
            return dt;
        }

        public DataTable FilterHotelsByAmenities(string amenities)
        {
            string filterHotelsByAmenitiesQuery = @"SELECT * FROM [SmartHotelDB].[dbo].[Hotel]
                                                    WHERE [Amenities] LIKE '%' + @Amenities + '%' AND [IsActive] = 1";
            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@Amenities", amenities)
            };
            DataTable dt = _dalObject.FetchData(filterHotelsByAmenitiesQuery, nvp);
            return dt;
        }

        public DataTable ReadHotelsByAvailability(DateTime startDate, DateTime endDate)
        {
            string readHotelsByAvailabilityQuery = @"SELECT DISTINCT H.*
                                                     FROM [SmartHotelDB].[dbo].[Hotel] H
                                                     JOIN [SmartHotelDB].[dbo].[Room] R ON H.HotelID = R.HotelID
                                                     WHERE R.RoomID NOT IN (
                                                         SELECT B.RoomID
                                                         FROM [SmartHotelDB].[dbo].[Bookings] B
                                                         WHERE B.CheckInDate <= @EndDate AND B.CheckOutDate >= @StartDate
                                                     ) AND H.[IsActive] = 1";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };
            DataTable dt = _dalObject.FetchData(readHotelsByAvailabilityQuery, parameters);
            return dt;
        }

        //public List<Hotel> ConvertDataTableToList(DataTable dataTable)
        //{
        //    var hotelList = new List<Hotel>();
        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        try
        //        {
        //            var hotel = new Hotel
        //            {
        //                HotelID = row.Field<int?>("HotelID") ?? 0,
        //                Name = row.Field<string>("Name") ?? string.Empty,
        //                Location = row.Field<string>("Location") ?? string.Empty,
        //                ManagerID = row.Field<int?>("ManagerID") ?? 0,
        //                Amenities = row.Field<string>("Amenities") ?? string.Empty,
        //                Rating = row.Field<double?>("Rating") ?? 0,
        //                IsActive = row.Field<bool?>("IsActive") ?? false
        //            };
        //            hotelList.Add(hotel);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception($"Error converting DataRow to Hotel: {ex.Message}", ex);
        //        }
        //    }
        //    return hotelList;
        //}

        public List<Hotel> GetAllHotels()
        {
            string fetchHotelsQuery = "SELECT * FROM [SmartHotelDB].[dbo].[Hotel] WHERE [IsActive] = 1";
            DataTable dt = _dalObject.FetchData(fetchHotelsQuery);
            List<Hotel> hotelList = ConvertDataTableToList(dt);
            return hotelList;
        }
    }
}
