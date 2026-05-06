# Commercial Runner UI

## Summary

Upgrade the prototype HUD into a polished mobile runner UI inspired by Count Masters: compact top-center level/progress treatment, prominent crowd count badge, tap-to-start prompt, and commercial-looking end-state panels.

## Motivation

The current prototype UI is sparse and uses plain text directly over gameplay. A stronger HUD will make the game read closer to a shipped hyper-casual runner while keeping gameplay visible and touch-friendly.

## Scope

- Replace the plain counter presentation with a rounded badge showing current snow power/crowd count.
- Add a top-center level header with previous/next level labels and a filled progress bar.
- Add a tap-to-start overlay that hides during play.
- Restyle game-over, level-clear, all-clear, and retry UI into centered modal panels.
- Create the Canvas hierarchy through Unity editor code so scene references are wired consistently.

## Non-Goals

- No new gameplay mechanics.
- No paid third-party UI packages.
- No legacy Input Manager changes.
- No manual `.meta` editing.
