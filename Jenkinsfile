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
        dotnetTest(logger: '\\"junit;LogFilePath=\\"${WORKSPACE}\\"/TestResults/1.0.0.\\"${env.BUILD_NUMBER}\\"/results.xml\\" --configuration release --collect \\"Code coverage\\""')
      }
    }

    stage('Coverage ') {
      steps {
        powershell 'CodeCoverage.exe analyze output:${WORKSPACE}\\\\\\CoursesLibrary_Main\\\\xmlresults.coveragexml  ${WORKSPACE}\\\\\\CoursesLibrary_Main\\\\testcoverage.coverage'
      }
    }

  }
}