# Design

## Approach

The implementation keeps the existing `UICounter` and `UIProgressBar` runtime roles so current gameplay code remains compatible. A new editor utility builds the commercial HUD hierarchy in `GameScene`, assigns references, and saves the scene.

## UI Layout

- Top center: `Level N` label with a white rounded progress track and blue fill.
- Top left: compact coin-style badge for snow power count.
- Center lower: `TAP TO START` prompt with semi-transparent readability plate.
- End states: centered dark translucent panel with large result text and yellow retry button.

## Runtime Behavior

- `UICounter` continues subscribing to `CrowdController.OnCrowdCountChanged`.
- `UICounter` hides the ready prompt when game state leaves `Ready`.
- `UICounter` can show the existing game-over text object for compatibility.
- `UIProgressBar` continues using `GameManager.TotalDistanceTraveled` and `SetLevelLength`.

## Scene Authoring

`CommercialRunnerUIBuilder` creates Unity UI primitives using `Text`, `Image`, `Button`, `CanvasScaler`, and `GraphicRaycaster`. It uses generated solid-color sprites for rounded-style panels where practical and avoids editing `.meta` files directly.

## Risks

- Unity legacy `Text` is retained to match existing code and scene dependencies.
- The builder is editor-only and should be removed or ignored at runtime.
