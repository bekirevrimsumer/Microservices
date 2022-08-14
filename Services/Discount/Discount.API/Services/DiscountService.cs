using System.Data;
using Dapper;
using Discount.API.Interfaces;
using Npgsql;
using Shared.Dtos;

namespace Discount.API.Services;

public class DiscountService : IDiscountService
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _dbConnection;
    
    public DiscountService(IConfiguration configuration)
    {
        _configuration = configuration;
        _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
    }
    
    public async Task<Response<List<Models.Discount>>> GetAll()
    {
        var discount = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount");
        return Response<List<Models.Discount>>.Success(discount.ToList(), 200);
    }

    public async Task<Response<Models.Discount>> GetById(int id)
    {
        var discount = (await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where id = @id", new {id})).SingleOrDefault();
        return discount == null ? Response<Models.Discount>.Error("Discount not found", 404) : Response<Models.Discount>.Success(discount, 200);
    }

    public async Task<Response<NoContent>> Create(Models.Discount discount)
    {
        var status = await _dbConnection.ExecuteAsync("Insert into discount (userid, rate, code) values (@UserId, @Rate, @Code)", discount); 
        return status == 0 ? Response<NoContent>.Error("Discount not created", 500) : Response<NoContent>.Success(200);
    }

    public async Task<Response<NoContent>> Update(Models.Discount discount)
    {
        var status = await _dbConnection.ExecuteAsync("Update discount set userid = @UserId, rate = @Rate, code = @Code where id = @Id", discount);
        return status == 0 ? Response<NoContent>.Error("Discount not found", 404) : Response<NoContent>.Success(200);
    }

    public async Task<Response<NoContent>> Delete(int id)
    {
        var status = await _dbConnection.ExecuteAsync("Delete from discount where id = @id", new {id});
        return status == 0 ? Response<NoContent>.Error("Discount not found", 404) : Response<NoContent>.Success(200);
    }

    public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
    {
        var discount = (await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where code = @code and userid = @userid", new {code, userId})).FirstOrDefault();
        return discount == null ? Response<Models.Discount>.Error("Discount not found", 404) : Response<Models.Discount>.Success(discount, 200);
    }
}