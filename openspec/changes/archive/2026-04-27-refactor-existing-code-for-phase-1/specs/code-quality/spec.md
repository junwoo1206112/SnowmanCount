# Code Quality & Style

## 요구사항
1. 모든 스크립트는 Allman 중괄호 스타일을 따라야 한다 (여는 중괄호 새 줄)
2. 들여쓰기는 4칸 공백이며 탭 사용 금지
3. 네임스페이스는 AGENTS.md 표에 따라 폴더별로 일치해야 한다
4. private 필드는 camelCase (밑줄 없음), SerializeField도 camelCase
5. 모든 Debug.Log는 `$"[ClassName]"` 접두사를 사용해야 한다
6. null 체크는 early return 패턴으로 처리한다
7. Using 문 순서: System → UnityEngine → NPOI → 프로젝트 네임스페이스
