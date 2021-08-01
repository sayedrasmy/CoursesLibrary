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
        bat 'CodeCoverage.exe analyze output:${WORKSPACE}\\\\TestResults\\\\xmlresults.coveragexml  ${WORKSPACE}\\\\TestResults\\\\testcoverage.coverage'
      }
    }

  }
}