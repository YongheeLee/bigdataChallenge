# 채용 정보 빅데이터 챌린지

데이터 분석에 활용한 코드들을 모아놓은 repository입니다.

상세한 내용은 아래 블로거에 정리할 예정입니다.
https://velog.io/@mir21c/series/%EB%B9%85%EB%8D%B0%EC%9D%B4%ED%84%B0%EC%B1%8C%EB%A6%B0%EC%A7%80

## 뷰어
DataViewer폴더에 BigDataChal.sln 실행
(VS 2019, .NET 4.7.2 기본 환경입니다)

### 데이터 연결
아래 사이트에 가서 로그인을 한 다음
상단의 '챌린지'에 들어가서 '디지털 산업 혁신 플랫폼 - 채용 정보 빅데이터 챌린지'을 클릭하면
하단에 데이터 CSV파일이 있습니다. 그걸 다운로드 하세요
http://new.bigdata-dx.kr/login

그리고 솔루션(BigDataChal.sln)을 열어보면
app.config파일이 있는데
아래와 같이 appsettings에 해당되는 파일 경로를 적어주세요

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
    </startup>
	<appSettings>
		<add key="company" value="D:\\공모전\\채용정보빅데이터챌린지\\datasetFiles\\알리콘(로켓펀치)-기업정보 - 기업정보.csv"/>
		<add key="service" value="D:\\공모전\\채용정보빅데이터챌린지\\datasetFiles\\알리콘(로켓펀치)-서비스제품정보.csv"/>
		<add key="job" value="D:\\공모전\\채용정보빅데이터챌린지\\datasetFiles\\알리콘(로켓펀치)-채용정보.csv"/>
	</appSettings>
</configuration>
```
실행 후, 상단에 Load 버튼을 클릭하면 일단 불러옵니다.

앞으로 분석을 위한 기능을 계속 추가할 예정입니다.

### konlpy 설치
https://konlpy.org/ko/v0.5.2/install/#id2
위 공식 문서를 참조함.

하지만 몇 가지 추가적으로 해야 함
참고로 내가 설치한 python 3.8.3임

일단 java는 1.7q버전으로 설치를 했으며 환경 변수에 JAVA_HOME까지는 완료

아래에서 jpype를 받아야 하는데,
JPype1‑1.1.2‑cp38‑cp38‑win_amd64.whl 을 받아야 함.
난 파이썬이 3.8.x이므로 cp38그리고 OS는 64비트이기 때문에 win_amd64
마지막으로 jpype버전은 1.1.2로 받음 상위를 받을 경우 에러 발생
https://www.lfd.uci.edu/~gohlke/pythonlibs/#jpype

그리고 konlpy를 설치하고 import 시에 tweety관련 에러가 발생함.
tweety버전이 4.4.0이였는데
`pip install tweety==3.10.0 --force-reinstall `
로 강제로 3.10버전을 설치함.

그럼 잘됨.
