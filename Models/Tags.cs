namespace Mss.App.Logger.Models;

/// <summary>
/// Represents a tag entity in the logging system. Each tag is used to categorize or label log entries.
/// </summary>
/// <remarks>
/// This class contains an immutable Name property that is set upon creation. 
/// It inherits from BaseModel, ensuring each tag has a unique identifier.
/// </remarks>
public class Tags : BaseModel
{
    /// <summary>
    /// Gets the name of the tag.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="Tags"/> class without specified name.
    /// </summary>
    public Tags() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Tags"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the tag.</param>
    public Tags(string name)
    {
        Name = name;
    }
}

