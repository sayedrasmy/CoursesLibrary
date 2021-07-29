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

    stage('Code coverage') {
      steps {
        bat 'dotnet add package Microsoft.CodeCoverage --version 16.10.0'
      }
    }

  }
}