pipeline {
  agent any
  stages {
    stage('error') {
      steps {
        dotnetBuild()
      }
    }

    stage('Unit test') {
      steps {
        dotnetTest()
      }
    }

    stage('Coverage ') {
      steps {
        powershell 'CodeCoverage.exe analyze output:${WORKSPACE}\\\\\\CoursesLibrary_Main\\\\xmlresults.coveragexml  ${WORKSPACE}\\\\\\CoursesLibrary_Main\\\\testcoverage.coverage'
      }
    }

  }
}