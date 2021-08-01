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
        powershell 'CodeCoverage.exe analyze output:${WORKSPACE}\\\\TestResults\\\\xmlresults.coveragexml  ${WORKSPACE}\\\\TestResults\\\\testcoverage.coverage"'
      }
    }

  }
}