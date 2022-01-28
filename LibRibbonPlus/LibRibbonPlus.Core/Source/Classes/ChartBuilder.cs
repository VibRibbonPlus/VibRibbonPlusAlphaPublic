// <copyright file="ChartBuilder.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Builds a <see cref="Chart"/> instance from a <see cref="string"/> or file.
/// </summary>
public sealed class ChartBuilder
{
    private readonly List<Note> _notes = new();

    private readonly List<BeatsPerMinute> _beatsPerMinutes = new();

    private ChartBuilderState _state;

    private string? _artist;

    private string? _charter;

    private string? _difficulty;

    private string? _directory;

    private string? _title;

    private ChartFormatVersion _version;

    /// <summary>
    /// Parses content into an instance of <see cref="Chart"/>.
    /// </summary>
    /// <param name="contents">The contents.</param>
    /// <returns>A modified <see cref="ChartBuilder"/> instance based on the data of <paramref name="contents"/>.</returns>
    public static ChartBuilder Parse(string contents) => Parse(contents.Split('\n'));

    /// <summary>
    /// Parses content into an instance of <see cref="ChartBuilder"/>.
    /// </summary>
    /// <param name="contents">The contents.</param>
    /// <returns>A modified <see cref="ChartBuilder"/> instance based on the data of <paramref name="contents"/>.</returns>
    public static ChartBuilder Parse(string[] contents)
    {
        ChartBuilder builder = new();

        contents.ForEach(s => builder.ApplyData(s));

        return builder;
    }

    /// <summary>
    /// Parses content into an instance of <see cref="Chart"/>.
    /// </summary>
    /// <param name="contents">The contents.</param>
    /// <returns>A modified <see cref="ChartBuilder"/> instance based on the data of <paramref name="contents"/>.</returns>
    public static ChartBuilder ParseOsu(string contents) => ParseOsu(contents.Split('\n'));

    /// <summary>
    /// Parses content into an instance of <see cref="ChartBuilder"/>.
    /// </summary>
    /// <param name="contents">The contents.</param>
    /// <returns>A modified <see cref="ChartBuilder"/> instance based on the data of <paramref name="contents"/>.</returns>
    public static ChartBuilder ParseOsu(string[] contents)
    {
        ChartBuilder builder = new();

        contents.ForEach(s => builder.ApplyOsuData(s));

        return builder;
    }

    /// <summary>
    /// Parses a file into an instance of <see cref="Chart"/>.
    /// </summary>
    /// <param name="directory">The file's location.</param>
    /// <returns>A modified <see cref="ChartBuilder"/> instance based on the data of <paramref name="directory"/>.</returns>
    public static async Task<ChartBuilder?> ParseAsync(Directory directory)
    {
        string? contents = await directory.ReadAsync().ConfigureAwait(false);

        return contents is { Length: >= 1 }
            ? Parse(contents)
            : null;
    }

    /// <summary>
    /// Parses a file into an instance of <see cref="Chart"/>.
    /// </summary>
    /// <param name="directory">The file's location.</param>
    /// <returns>A modified <see cref="ChartBuilder"/> instance based on the data of <paramref name="directory"/>.</returns>
    public static async Task<ChartBuilder?> ParseOsuAsync(Directory directory)
    {
        string? contents = await directory.ReadAsync().ConfigureAwait(false);

        return contents is { Length: >= 1 }
            ? ParseOsu(contents)
            : null;
    }

    /// <summary>
    /// Builds the chart.
    /// </summary>
    /// <returns>A value of <see cref="Chart"/> if it is valid, or <see langword="null"/>.</returns>
    public Chart Build()
        => new(
            new(Sort(_notes)),
            new(Sort(_beatsPerMinutes)),
            new(
                new(
                    new(_title ?? string.Empty),
                    new(_artist ?? string.Empty),
                    new(_directory ?? string.Empty)),
                new(
                    new(_charter ?? string.Empty),
                    _difficulty.ToDifficulties()),
                _version));

    /// <summary>
    /// Appends one or more of <see cref="BeatsPerMinute"/>.
    /// </summary>
    /// <param name="beatsPerMinutes">The beats per minute changes to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder And(params BeatsPerMinute[] beatsPerMinutes)
        => And((IEnumerable<BeatsPerMinute>)beatsPerMinutes);

    /// <summary>
    /// Appends one or more of <see cref="BeatsPerMinute"/>.
    /// </summary>
    /// <param name="beatsPerMinutes">The beats per minute changes to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder And(IEnumerable<BeatsPerMinute> beatsPerMinutes)
    {
        _beatsPerMinutes.AddRange(beatsPerMinutes);
        return this;
    }

    /// <summary>
    /// Appends one or more of <see cref="Note"/>.
    /// </summary>
    /// <param name="notes">The notes to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder And(params Note[] notes)
        => And((IEnumerable<Note>)notes);

    /// <summary>
    /// Appends one or more of <see cref="Note"/>.
    /// </summary>
    /// <param name="notes">The notes to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder And(IEnumerable<Note> notes)
    {
        _notes.AddRange(notes);
        return this;
    }

    /// <summary>
    /// Appends one or more of <see cref="BeatsPerMinute"/>.
    /// </summary>
    /// <param name="beatsPerMinutes">The beats per minute changes to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder AndNot(params BeatsPerMinute[] beatsPerMinutes)
        => AndNot((IEnumerable<BeatsPerMinute>)beatsPerMinutes);

    /// <summary>
    /// Appends one or more of <see cref="BeatsPerMinute"/>.
    /// </summary>
    /// <param name="beatsPerMinutes">The beats per minute changes to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder AndNot(IEnumerable<BeatsPerMinute> beatsPerMinutes)
    {
        beatsPerMinutes.ForEach(n => _beatsPerMinutes.Remove(n));
        return this;
    }

    /// <summary>
    /// Appends one or more of <see cref="Note"/>.
    /// </summary>
    /// <param name="notes">The notes to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder AndNot(params Note[] notes)
        => AndNot((IEnumerable<Note>)notes);

    /// <summary>
    /// Appends one or more of <see cref="Note"/>.
    /// </summary>
    /// <param name="notes">The notes to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder AndNot(IEnumerable<Note> notes)
    {
        notes.ForEach(n => _notes.Remove(n));
        return this;
    }

    /// <summary>
    /// Inserts the artist.
    /// </summary>
    /// <param name="artist">The artist to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder WithArtist(Label artist)
    {
        _artist = artist.Name;
        return this;
    }

    /// <summary>
    /// Inserts the charter.
    /// </summary>
    /// <param name="charter">The charter to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder WithCharter(Label charter)
    {
        _charter = charter.Name;
        return this;
    }

    /// <summary>
    /// Inserts the <see cref="ChartFormatVersion"/>.
    /// </summary>
    /// <param name="version">The version to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder WithVersion(ChartFormatVersion version)
    {
        _version = version;
        return this;
    }

    /// <summary>
    /// Inserts <see cref="Difficulties"/>.
    /// </summary>
    /// <param name="difficulty">The difficulty to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder WithDifficulty(Difficulties difficulty)
    {
        _difficulty = difficulty.ToString();
        return this;
    }

    /// <summary>
    /// Inserts the <see cref="Directory"/>.
    /// </summary>
    /// <param name="songDirectory">The song directory to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder WithDirectory(Directory songDirectory)
    {
        _directory = songDirectory.Path;
        return this;
    }

    /// <summary>
    /// Inserts the title.
    /// </summary>
    /// <param name="title">The title to add.</param>
    /// <returns>Itself.</returns>
    public ChartBuilder WithTitle(Label title)
    {
        _title = title.Name;
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Test(string line, FormatTryer format)
    {
        (string prefix, Action<string> apply) = format;

        if (line.StartsWith(prefix, StringComparison.Ordinal))
            apply(line.Substring(prefix.Length));
    }

    private static Unit TestForAny(string line, params FormatTryer[] formats)
    {
        formats.ForEach(f => Test(line, f));
        return default;
    }

    private static T[] Sort<T>(List<T> list)
        where T : ITimed
        => list.OrderBy(n => n.Interval.Microseconds).ToArray();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ApplyData(Chars line)
    {
        TestForAny(
            line.ToString(),
            (Chart.MetadataTag, _ => _state = ChartBuilderState.IsProcessingMetadata),
            (Chart.BeatsPerMinuteTag, _ => _state = ChartBuilderState.IsProcessingBeatsPerMinute),
            (Chart.NoteTag, _ => _state = ChartBuilderState.IsProcessingNotes));

        _ = _state switch
        {
            ChartBuilderState.IsProcessingVersion => ProcessVersion(line),
            ChartBuilderState.IsProcessingMetadata => ProcessMetadata(line),
            ChartBuilderState.IsProcessingBeatsPerMinute => ProcessBeatsPerMinute(line),
            ChartBuilderState.IsProcessingNotes => ProcessNotes(line),
            _ => default,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ApplyOsuData(Chars line)
    {
        TestForAny(
            line.ToString(),
            ("[General]", _ => _state = ChartBuilderState.IsProcessingMetadata),
            ("[Metadata]", _ => _state = ChartBuilderState.IsProcessingMetadata),
            ("[TimingPoints]", _ => _state = ChartBuilderState.IsProcessingBeatsPerMinute),
            ("[HitObjects]", _ => _state = ChartBuilderState.IsProcessingNotes));

        _ = _state switch
        {
            ChartBuilderState.IsProcessingVersion => ProcessOsuVersion(line),
            ChartBuilderState.IsProcessingMetadata => ProcessOsuMetadata(line),
            ChartBuilderState.IsProcessingBeatsPerMinute => ProcessOsuBeatsPerMinute(line),
            ChartBuilderState.IsProcessingNotes => ProcessOsuNotes(line),
            _ => default,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Unit ProcessVersion(Chars line)
    {
        if (ChartFormatVersion.TryParse(line, out ChartFormatVersion version))
            _version = version;

        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Unit ProcessOsuVersion(Chars line)
    {
        if (line.Length > 17 &&
            line[0..17] == "osu file format v" &&
            uint.TryParse(line[17..].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out uint result))
            _version = new(result);

        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Unit ProcessMetadata(Chars line)
        => TestForAny(
            line.ToString(),
            (Chart.ArtistTag, s => _artist = s),
            (Chart.CharterTag, s => _charter = s),
            (Chart.DifficultyTag, s => _difficulty = s),
            (Chart.DirectoryTag, s => _directory = s),
            (Chart.TitleTag, s => _title = s));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Unit ProcessOsuMetadata(Chars line)
        => TestForAny(
            line.ToString(),
            ("Artist:", s => _artist = s),
            ("Creator:", s => _charter = s),
            ("Version:", s => _difficulty = s),
            ("AudioFilename: ", s => _directory = s),
            ("Title:", s => _title = s));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Unit ProcessBeatsPerMinute(Chars line)
    {
        if (BeatsPerMinute.TryParse(line, out BeatsPerMinute beatsPerMinute))
            _beatsPerMinutes.Add(beatsPerMinute);

        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Unit ProcessOsuBeatsPerMinute(Chars line)
    {
        if (line.ToString().Split(',') is { Length: 8 } split &&
            Interval.TryParse(split[0], out Interval time) &&
            float.TryParse(split[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float tempo))
            _beatsPerMinutes.Add(new(tempo, time));

        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Unit ProcessNotes(Chars line)
    {
        if (Note.TryParse(line, out Note note))
            _notes.Add(note);

        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Unit ProcessOsuNotes(Chars line)
    {
        // todo!
        if (line.ToString().Split(',') is { Length: 6 } split &&
            Interval.TryParse(split[0], out Interval time) &&
            float.TryParse(split[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float tempo))
            _beatsPerMinutes.Add(new(tempo, time));

        return default;
    }
}
