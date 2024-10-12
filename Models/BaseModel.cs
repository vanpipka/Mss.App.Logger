using System.ComponentModel.DataAnnotations;

namespace Mss.App.Logger.Models;

/// <summary>
/// Base class for models in paket
/// </summary>

public class BaseModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the log entry.
    /// </summary>
    [Key]
    public Guid Id { get; private set; }

    /// <summary>
    /// Constructor for BaseModel.
    /// </summary>
    public BaseModel(Guid? id = default)
    {
        Id = id ?? Guid.NewGuid();
    }
}
