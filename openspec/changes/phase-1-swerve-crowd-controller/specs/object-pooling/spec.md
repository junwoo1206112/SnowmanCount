# Object Pooling

## 요구사항
1. ObjectPooler는 Queue 기반으로 눈사람 프리팹을 관리한다
2. 풀에서 가져올 때는 SetActive(true), 반환할 때는 SetActive(false)
3. 초기 풀 크기는 50으로 설정한다
4. 풀이 부족하면 자동으로 새 인스턴스를 생성한다
5. CrowdController는 병력 생성/소멸 시 ObjectPooler를 통해 처리한다
