using Rop.Result;

namespace UnitTest;

using Xunit;
using System.Collections.Generic;
using System.Linq;

public class ResultActionTests
{
    public static IEnumerable<object[]> ErrorTestCases()
    {
        yield return new object[] { null!, ResultCode.Ok, true };
        yield return new object[] { new List<ErrorValidation>(), ResultCode.NotFound, false };
        yield return new object[] { new List<ErrorValidation> { new("error") }, ResultCode.BadRequest, false };
    }

    [Theory]
    [MemberData(nameof(ErrorTestCases))]
    public void IsSuccess_ShouldReturnCorrectValue(
        List<ErrorValidation> errors,
        ResultCode expectedStatusCode,
        bool expectedIsSuccess)
    {
        // Arrange
        var result = errors switch
        {
            null => ResultAction.Success(),
            var e when e.Count == 0 => ResultAction.NotFound("test"),
            _ => ResultAction.BadRequest(errors.First(), "Error")
        };

        // Assert
        Assert.Equal(expectedIsSuccess, result.IsSuccess);
        Assert.Equal(expectedStatusCode, result.StatusCode);
    }

    [Fact]
    public void Success_ShouldCreateCorrectResult()
    {
        // Act
        var result = ResultAction.Success("Éxito");

        // Assert
        Assert.Equal(ResultCode.Ok, result.StatusCode);
        Assert.Equal("Éxito", result.Message);
        Assert.Null(result.ValidationErrors);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void BadRequest_WithSingleError_ShouldContainError()
    {
        // Arrange
        var error = new ErrorValidation("Campo", "Requerido");

        // Act
        var result = ResultAction.BadRequest(error);

        // Assert
        Assert.Equal(ResultCode.BadRequest, result.StatusCode);
        Assert.Single(result.ValidationErrors!);
        Assert.Equal(error, result.ValidationErrors!.First());
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void BadRequest_WithParameters_ShouldCreateErrorCorrectly()
    {
        // Act
        var result = ResultAction.BadRequest("Campo", "Requerido");

        // Assert
        Assert.Equal(ResultCode.BadRequest, result.StatusCode);
        var error = result.ValidationErrors!.First();
        Assert.Equal("Campo", error.Identifier);
        Assert.Equal("Requerido", error.ErrorMessage);
    }

    [Fact]
    public void NotFound_ShouldHaveEmptyErrorList()
    {
        // Act
        var result = ResultAction.NotFound("usuario");

        // Assert
        Assert.Equal(ResultCode.NotFound, result.StatusCode);
        Assert.Equal("No se encontro en recurso usuario", result.Message);
        Assert.NotNull(result.ValidationErrors);
        Assert.Empty(result.ValidationErrors!);
    }

    [Fact]
    public void Unauthorized_ShouldHaveCorrectStatusCode()
    {
        // Act
        var result = ResultAction.Unauthorized("perfil");

        // Assert
        Assert.Equal(ResultCode.Unauthorized, result.StatusCode);
        Assert.Contains("perfil", result.Message);
        Assert.NotNull(result.ValidationErrors);
        Assert.Empty(result.ValidationErrors!);
    }

    [Fact]
    public void EmptyErrorList_ShouldNotBeBadRequest()
    {
        // Act
        var result = ResultAction.NotFound("test");

        // Assert
        Assert.NotEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.NotNull(result.ValidationErrors);
        Assert.Empty(result.ValidationErrors!);
    }
}

public class ErrorValidationTests
{
    [Fact]
    public void ErrorValidation_ShouldStoreProperties()
    {
        // Arrange
        var error = new ErrorValidation("id", "mensaje");

        // Assert
        Assert.Equal("id", error.Identifier);
        Assert.Equal("mensaje", error.ErrorMessage);
    }

    [Fact]
    public void EmptyErrorList_ShouldBeEmpty()
    {
        // Act
        var emptyList = ErrorValidation.Empty();

        // Assert
        Assert.NotNull(emptyList);
        Assert.Empty(emptyList);
    }
}

public class ResultCodeTests
{
    [Theory]
    [InlineData(200, "OK")]
    [InlineData(400, "Bad Request")]
    [InlineData(404, "Not Found")]
    public void ResultCode_ShouldHaveCorrectValues(int id, string name)
    {
        // Act
        var resultCode = GetResultCode(id);

        // Assert
        Assert.Equal(id, resultCode.Id);
        Assert.Equal(name, resultCode.Name);
    }

    private ResultCode GetResultCode(int id) => id switch
    {
        200 => ResultCode.Ok,
        400 => ResultCode.BadRequest,
        401 => ResultCode.Unauthorized,
        403 => ResultCode.Forbidden,
        404 => ResultCode.NotFound,
        500 => ResultCode.InternalServerError,
        501 => ResultCode.NotImplemented,
        _ => throw new KeyNotFoundException()
    };
}
