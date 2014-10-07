﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        readonly List<City> _cities = new List<City>();
        public int Count { get { return _cities.Count; } }

        /// <summary>
        /// Read cities from text file
        /// One entry per line, seperated with a tab (name, country, population, latitude, longitude)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public int ReadCities(string filename)
        {
            var readCounter = 0;
            try
            {
                using (var sr = new StreamReader(filename))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var data = line.Split('\t');
                        _cities.Add(new City(data[0], data[1], Int32.Parse(data[2]), Double.Parse(data[3], CultureInfo.InvariantCulture), Double.Parse(data[4], CultureInfo.InvariantCulture)));
                        ++readCounter;
                    }
                }
                return readCounter;
            }
            catch (Exception e)
            {
                Console.WriteLine("The file \"" + filename + "\" could not be read!");
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        /// <summary>
        /// Find all neighbours of a city within the defined distance.
        /// The list is sorted by distance 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public List<City> FindNeighbours(WayPoint location, double distance)
        {
            var neighbours = new SortedDictionary<double, City>();
            foreach (var c in _cities)
            {
                var currentDistance = location.Distance(c.Location);
                if (currentDistance <= distance)
                {
                    neighbours.Add(currentDistance, c);
                }
            }
            return neighbours.Values.ToList();
        }

        /// <summary>
        /// Iterator for read cities
        /// </summary>
        /// <param name="index">Index of the city to select</param>
        /// <returns>City with defined index</returns>
        public City this[int index]
        {
            get { return (index < _cities.Count) ? _cities[index] : null; }
        }

        /// <summary>
        /// Find a city object with specified name
        /// </summary>
        /// <param name="cityName">Name of the city to find</param>
        /// <returns></returns>
        public City FindCity(string cityName)
        {
            return _cities.Find(delegate(City c)
            {
                if (c.Name.ToLower().Equals(cityName.ToLower()))
                    return true;
                return false;
            });
        }

        #region Lab04: FindShortestPath helper function
        /// <summary>
        /// Find all cities between 2 cities 
        /// </summary>
        /// <param name="from">source city</param>
        /// <param name="to">target city</param>
        /// <returns>list of cities</returns>
        public List<City> FindCitiesBetween(City from, City to)
        {
            var foundCities = new List<City>();
            if (from == null || to == null)
                return foundCities;

            foundCities.Add(from);

            var minLat = Math.Min(from.Location.Latitude, to.Location.Latitude);
            var maxLat = Math.Max(from.Location.Latitude, to.Location.Latitude);
            var minLon = Math.Min(from.Location.Longitude, to.Location.Longitude);
            var maxLon = Math.Max(from.Location.Longitude, to.Location.Longitude);

            foundCities.AddRange(_cities.FindAll(c =>
                c.Location.Latitude > minLat && c.Location.Latitude < maxLat
                        && c.Location.Longitude > minLon && c.Location.Longitude < maxLon));

            foundCities.Add(to);
            return foundCities;
        }
        #endregion
    }
}
