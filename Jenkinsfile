import groovy.transform.Field

@Field String newTag = ''

pipeline {
    agent any

    parameters {
        string(name: 'BRANCH', defaultValue: 'main_1.0', description: 'Git branch to build')
        choice(name: 'RELEASE_TYPE', choices: ['major', 'minor'], description: 'Select the release type for build')
    }

    environment {
        NUGET_SOURCE = "https://nuget.pkg.github.com/rajalingamp66/index.json"
    }

    stages {
        stage('Check Git Access') {
            steps {
                git credentialsId: 'github-nice-cxone',
                    url: 'https://github.com/rajalingamp66/PipelineNuget-CustomConfigurationManager.git',
                    branch: "${params.BRANCH}"
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
                    echo 'Building the NuGet package...'
                    withEnv(["newTag=${newTag}"]) {
                        bat """
                            dotnet build --configfile src\\.nuget\\NuGet.Config -c Release src\\PipelineNuget-CustomConfigurationManager.sln
                            echo 'Packing...'
                            dotnet pack -c Release -p:Version=%newTag% -o ./nupkgs src\\PipelineNuget-CustomConfigurationManager.sln
                        """
                    }
                }
            }
        }

        stage('Push to NuGet') {
            steps {
                script {
                    echo 'Pushing NuGet package...'
                    withCredentials([string(credentialsId: 'github-packages-read-write', variable: 'GITHUB_TOKEN1')]) {
                        bat """
                            echo Pushing to NuGet using token...
                            dotnet nuget push .\\nupkgs\\*.nupkg ^
                                -k %GITHUB_TOKEN1% ^
                                -s https://nuget.pkg.github.com/rajalingamp66/index.json ^
                                --skip-duplicate ^
                                --verbosity detailed
                        """
                    }
                }
            }
        }

        stage('Push Git Tag') {
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'github-nice-cxone', usernameVariable: 'GIT_USERNAME', passwordVariable: 'GIT_PASSWORD')]) {
                        withEnv(["newTag=${newTag}"]) {
                            bat """
                                echo Pushing tag: %newTag%

                                git config --local credential.helper "!f() { echo username=\\%GIT_USERNAME%; echo password=\\%GIT_PASSWORD%; }; f"
                                git config user.email "jenkins@vj-linux"
                                git config user.name "jenkins"

                                git tag -a %newTag% -m "jenkins ci auto commit"
                                git push origin refs/tags/%newTag%
                            """
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
