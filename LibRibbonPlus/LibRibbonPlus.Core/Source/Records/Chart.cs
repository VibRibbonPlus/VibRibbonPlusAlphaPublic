// <copyright file="Chart.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents an entire object of a chart or level.
/// </summary>
/// <param name="Notes">The collection of notes.</param>
/// <param name="BeatsPerMinuteChanges">The collection of beats per minute changes.</param>
/// <param name="Metadata">Any chart metadata.</param>
public sealed record Chart([property: JsonIgnore] NoteCollection Notes, [property: JsonIgnore] BeatsPerMinuteCollection BeatsPerMinuteChanges, Metadata Metadata)
{
    /// <summary>
    /// Contains the metadata tag used to switch context to metadata.
    /// </summary>
    public const string MetadataTag = "[Metadata]";

    /// <summary>
    /// Contains the beats per minute tag used to switch context to metadata.
    /// </summary>
    public const string BeatsPerMinuteTag = "[BPMs]";

    /// <summary>
    /// Contains the note tag used to switch context to notes.
    /// </summary>
    public const string NoteTag = "[Notes]";

    /// <summary>
    /// Contains the artist tag used to specify the <see cref="SongMetadata.Artist"/>.
    /// </summary>
    public const string ArtistTag = "Artist: ";

    /// <summary>
    /// Contains the charter tag used to specify the <see cref="ChartMetadata.Charter"/>.
    /// </summary>
    public const string CharterTag = "Charter: ";

    /// <summary>
    /// Contains the difficulty tag used to specify <see cref="Difficulties"/>.
    /// </summary>
    public const string DifficultyTag = "Difficulty: ";

    /// <summary>
    /// Contains the directory tag used to specify <see cref="Directory"/>.
    /// </summary>
    public const string DirectoryTag = "Song Directory: ";

    /// <summary>
    /// Contains the title tag used to specify the <see cref="SongMetadata.Title"/>.
    /// </summary>
    public const string TitleTag = "Title: ";

    /// <summary>
    /// Gets the explicit default value of this type.
    /// </summary>
    public static Chart Blank { get; } = new(NoteCollection.Blank, BeatsPerMinuteCollection.Blank, Metadata.Blank);

    /// <summary>
    /// Gets the <see cref="SHA256"/> hash of this instance.
    /// </summary>
    [JsonIgnore]
    public string Sha256
    {
        get
        {
            using var sha256 = SHA256.Create();

            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(ToString()));

            StringBuilder builder = new();

            bytes.ForEach(b => builder.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", b));

            return builder.ToString();
        }
    }

    [JsonRequired]
    [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Newtonsoft.Json will grab this using System.Reflection.")]
    private Note[] NoteList => Notes.Collection;

    [JsonRequired]
    [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Newtonsoft.Json will grab this using System.Reflection.")]
    private BeatsPerMinute[] BPMList => BeatsPerMinuteChanges.Collection;

    /// <inheritdoc/>
    public override string ToString()
        => @$"{Metadata.Version}

{MetadataTag}
{TitleTag}{Metadata.Song.Title}
{ArtistTag}{Metadata.Song.Artist}
{DirectoryTag}{Metadata.Song.Directory}
{CharterTag}{Metadata.Chart.Charter}
{DifficultyTag}{Metadata.Chart.Difficulty}

{BeatsPerMinuteTag}
{BeatsPerMinuteChanges}
{NoteTag}
{Notes}";
}
