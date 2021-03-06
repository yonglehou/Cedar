﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;

namespace Cedar
{
    public class ShardStrategyRepository:BaseRepository 
    {
        /// <summary>
        /// Get all shard strategy
        /// </summary>
        /// <returns></returns>
        public IList<ShardWile> GetShardStrategy()
        {

            using (var connection = GetConnection())
            {
                string query = "Select Id,ShardId,TotalCount, MaxCount from shardwile" ;

            
                var shardPileList = connection.Query<ShardWile>(query);

                return shardPileList.ToList();
            }
        }
        /// <summary>
        /// Get the shrad strategy for a given shard
        /// </summary>
        /// <param name="shardId"></param>
        /// <returns></returns>
        public IList<ShardWile> GetShardStrategyById(long shardId)
        {

            using (var connection = GetConnection())
            {
                string query = "Select Id,ShardId,TotalCount, MaxCount from shardwile" +
                                     " WHERE ShardId = @ShardId";

                var parameters = new DynamicParameters();

                parameters.Add("@ShardId", shardId);

                var shardPileList = connection.Query<ShardWile>(query, parameters);

                return shardPileList.ToList();
            }
        }
        /// <summary>
        /// Updates the total counts of a shard
        /// </summary>
        /// <param name="shardId"></param>
        public void UpdateShardStrategy(long shardId)
        {

            using (var connection = GetConnection())
            {
                string query = "update ShardWile set TotalCount=(TotalCount)+1 WHERE ShardId = @SId";
                var parameters = new DynamicParameters();
                parameters.Add("ShardId", shardId);
               connection.Execute(query, parameters);
              

            }
        }
        /// <summary>
        /// Add new strategy
        /// </summary>
        /// <param name="shardId"></param>
        /// <param name="totalCount"></param>
        /// <param name="maxCount"></param>
        public void AddShardStrategy(long shardId,int totalCount,int maxCount)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    string query = "insert into ShardWile (shardId,TotalCount,MaxCount) values (@ShardId,@totalCount,@maxCount) ";

                    var parameters = new DynamicParameters();

                    parameters.Add("@ShardId", shardId, DbType.Int64);
                    parameters.Add("@totalCount", totalCount, DbType.Int32);
                    parameters.Add("@maxCount", maxCount, DbType.Int32);
                    var transaction = connection.BeginTransaction();
                    connection.Execute(query, parameters, transaction, null, System.Data.CommandType.Text);
                    transaction.Commit();
                    transaction.Dispose();
                }
            }
            catch (Exception e)
            {
               
            }
           
        }
    }
}
