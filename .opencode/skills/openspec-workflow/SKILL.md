---
name: openspec-workflow
description: OpenSpec spec-driven development workflow - propose, design, spec, task, apply, archive
license: MIT
compatibility: opencode
metadata:
  source: https://github.com/Fission-AI/OpenSpec
---

## 필수 워크플로우

모든 기능 추가/변경 작업은 반드시 이 워크플로우를 따라야 함.

### 1. 제안 (Propose)
```bash
/opsx:propose "기능명"
```
생성되는 파일:
- `openspec/changes/<name>/proposal.md` — 무엇을, 왜 변경하는지
- `openspec/changes/<name>/design.md` — 어떻게 구현할지
- `openspec/changes/<name>/specs/` — 변경되는 요구사항
- `openspec/changes/<name>/tasks.md` — 구현 체크리스트

### 2. 구현 (Apply)
```bash
/opsx:apply
```
tasks.md의 작업 항목을 순서대로 구현.
각 task 완료 시 `- [ ]` → `- [x]`로 표시.

### 3. 탐색 (Explore) - 아이디어 검토만 할 때
```bash
/opsx:explore
```
코드 작성 금지, 아이디어 검토만.

### 4. 보관 (Archive)
```bash
/opsx:archive
```
완료된 변경사항을 `openspec/changes/archive/`로 이동.
델타 스펙이 있으면 메인 스펙에 자동 병합됨.

## 예외 규칙
- **단순 버그 수정 (1-2줄)** : 바로 수정 가능, propose 생략
- **구조적 변경 필요** : 반드시 propose → apply 순서
- **작업 전** : 항상 `openspec list --json`으로 활성 변경사항 확인
