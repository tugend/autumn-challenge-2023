using JetBrains.Annotations;

namespace WebUi.Models;

[UsedImplicitly]
public record Seed(int Turn, int[][] Grid);