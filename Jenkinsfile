def ON_FAILURE_SEND_EMAIL = true
def ON_SUCCESS_SEND_EMAIL = true 
pipeline {
    agent any
    parameters {
        booleanParam(name: "CLEAN_WORKSPACE", defaultValue: false)
        booleanParam(name: "TESTING_FRONTENT", defaultValue: false)
    }
    
    stages {
        stage('Clean workspace') {
            when { expression { params.CLEAN_WORKSPACE } }
            steps {
                cleanWs()
            }
        }
        
        stage('Checkout Stage') {
            steps {
                git branch: 'main', credentialsId: '9e279393-bcb6-48fb-9a1f-32ce2f3785f9', url: 'https://github.com/ritartha017/bs-jenkins.git'
            }
        }
        
        stage('Restore packages') {
            steps {
                sh "sudo dotnet restore ${workspace}\\Endava.BookSharing.sln"
            }
        }
        
        stage('Build Stage') {
            steps {
                sh "dotnet msbuild ${workspace}\\Endava.BookSharing.sln /p:configuration=\"release\""
            }
        }
        
        stage('Test BE Stage') {
            steps {
                sh "dotnet test --logger:\"junit;LogFilePath=%WORKSPACE%\\TestResults\\dotnet-test-result.xml\""
            }
        }
        
        stage('Test FE Stage') {
            when { expression { params.TESTING_FRONTENT } }
            steps {
                echo "Testing frontent stage .."
                echo "${params.TESTING_FRONTENT}"
            }
        }
    }
    post {
        success {
            sendEmail(ON_SUCCESS_SEND_EMAIL)
        }
        failure {
            script {
                ON_FAILURE_SEND_EMAIL = false;
            }
            sendEmail(ON_FAILURE_SEND_EMAIL)
        }
    }
}

void sendEmail(Boolean result) {
    String success = "";
    if (result == true) {
        success = "SUCCEEDED";
    } else {
        success = "FAILED";
    }
    emailext body: "Build ${success}\n\nThe job name is ${env.JOB_NAME},\nThe build number is ${env.BUILD_NUMBER}\nThe build ID is ${env.BUILD_ID}", 
    recipientProviders: [[$class: 'DevelopersRecipientProvider'], [$class: 'RequesterRecipientProvider']], 
    subject: 'BuildStatus'
}
