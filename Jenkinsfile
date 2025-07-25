import groovy.transform.Field

@Field String newTag = ''

pipeline {
    agent any

    options {
        skipDefaultCheckout()
    }

    parameters {
        // Replaced gitParameter with standard string input
        string(name: 'BRANCH', defaultValue: 'main_1.0', description: 'Git Branch to build')
        choice(name: 'RELEASE_TYPE', choices: ['major', 'minor'], description: 'Select the release type for build')
    }

    environment {
        NUGET_SOURCE = "https://nuget.pkg.github.com/rajalingamp66/index.json"
        GITHUB_USERNAME = "Rajalingam.Periyathambi@nice.com"
        GITHUB_TOKEN = credentials("github-packages-read-write")
    }

    stages {

        stage('Checkout') {
            steps {
                git credentialsId: 'github-nice-cxone',
                    url: 'https://github.com/rajalingamp66/PipelineNuget-CustomConfigurationManager.git',
                    branch: "${params.BRANCH}"
            }
        }

        stage("Resolve Tag") {
            steps {
                script {
                    newTag = powershell(returnStdout: true, script: ".\\resolveGitChanges.ps1 ${params.RELEASE_TYPE} ${params.BRANCH}").trim()
                    echo "Resolved Tag: ${newTag}"
                }
            }
        }

        stage('Build & Pack') {
            steps {
                script {
                    echo "Building & Packing with tag ${newTag}"
                    bat """
                        cd PipelineNuget-CustomConfigurationManager\\src
                        dotnet build --configfile .nuget\\NuGet.Config PipelineNuget-CustomConfigurationManager.sln
                        dotnet pack -c Release -p:Version=${newTag} PipelineNuget-CustomConfigurationManager.sln
                    """
                }
            }
        }

        stage('Push to NuGet') {
            steps {
                bat """
                    dotnet nuget push PipelineNuget-CustomConfigurationManager\\src\\CustomConfigurationManager\\bin\\Release\\*.nupkg ^
                        -k ${env.GITHUB_TOKEN} ^
                        -s ${env.NUGET_SOURCE} ^
                        --skip-duplicate
                """
            }
        }

        stage('Push Git Tag') {
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'github-nice-cxone', usernameVariable: 'GIT_USERNAME', passwordVariable: 'GIT_PASSWORD')]) {
                        bat """
                            git config --local credential.helper "!f() { echo username=\\%GIT_USERNAME%; echo password=\\%GIT_PASSWORD%; }; f"
                            git config user.email "jenkins@vj-linux"
                            git config user.name "jenkins"
                            git tag -a ${newTag} -m "jenkins ci auto commit"
                            git push origin refs/tags/${newTag}
                        """
                    }
                }
            }
        }
    }

  post {
    always {
        script {
            currentBuild.description = "${params.RELEASE_TYPE} : ${newTag}"
        }

        node('BuildAgent01') {
            cleanWs()
        }
    }
}
}
