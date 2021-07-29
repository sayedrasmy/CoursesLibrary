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

  }
}