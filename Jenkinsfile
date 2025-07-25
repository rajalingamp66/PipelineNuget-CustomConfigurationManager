import groovy.transform.Field

@Field String newTag = ''

pipeline {
    agent any

    parameters {
        string(name: 'BRANCH', defaultValue: 'main_1.0', description: 'Git branch to build')
        choice(name: 'RELEASE_TYPE', choices: ['major', 'minor'], description: 'Select the release type for build')
    }

    environment {
        NUGET_SOURCE = "https://nuget.pkg.github.com/inContact/index.json"
        GITHUB_USERNAME = "cxossp"
        GITHUB_TOKEN = credentials("github-packages-read-write")
    }

    stages {
        stage('Checkout') {
            steps {
                checkout([
                    $class: 'GitSCM',
                    branches: [[name: "*/${params.BRANCH}"]],
                    userRemoteConfigs: [[
                        url: 'https://github.com/rajalingamp66/PipelineNuget-CustomConfigurationManager.git',
                        credentialsId: 'github-nice-cxone'
                    ]]
                ])
            }
        }

        stage("Preparation Stage") {
            steps {
                script {
                    newTag = powershell(returnStdout: true, script: ".\\resolveGitChanges.ps1 ${params.RELEASE_TYPE} ${params.BRANCH}").trim()
                    echo "Branch name: ${params.BRANCH}"
                    echo "New Tag generated: ${newTag}"
                }
            }
        }

        stage('Build') {
            steps {
                script {
                    echo 'Building the package'
                    withEnv(["newTag=${newTag}"]) {
                        bat '''
                            cd ./CustomConfigurationManager
                            dotnet build --configfile .nuget/NuGet.Config CustomConfigurationManager.sln
                            echo 'Packing the build'
                            cd ./CustomConfigurationManager
                            dotnet pack -c Release -p:Version=%newTag%
                        '''
                    }

                    echo 'Push package to NuGet'
                    bat '''
                        dotnet nuget push CustomConfigurationManager/CustomConfigurationManager/bin/Release/CustomConfigurationManager**.nupkg -k %GITHUB_TOKEN% -s %NUGET_SOURCE%
                    '''
                }
            }
        }

        stage('Push Tag') {
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'github-nice-cxone', usernameVariable: 'GIT_USERNAME', passwordVariable: 'GIT_PASSWORD')]) {
                        withEnv(["newTag=${newTag}"]) {
                            bat '''
                                echo "%newTag%"

                                git config --local credential.helper "!p() { echo username=\\%GIT_USERNAME%; echo password=\\%GIT_PASSWORD%; }; p"
                                git config user.email "jenkins@vj-linux"
                                git config user.name "jenkins"

                                git tag -m "jenkins ci auto commit" -a %newTag%
                                git push origin refs/tags/%newTag%
                            '''
                        }
                    }
                }
            }
        }
    }

    post {
        always {
            cleanWs()
            script {
                currentBuild.description = "${params.RELEASE_TYPE} : ${newTag}"
            }
        }
    }
}
