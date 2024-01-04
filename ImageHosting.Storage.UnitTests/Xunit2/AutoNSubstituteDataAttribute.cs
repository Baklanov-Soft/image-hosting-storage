using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace ImageHosting.Storage.UnitTests.Xunit2;

public class AutoNSubstituteDataAttribute()
    : AutoDataAttribute(() => new Fixture {RepeatCount = 3}.Customize(new AutoNSubstituteCustomization()));
