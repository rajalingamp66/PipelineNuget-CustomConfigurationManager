import groovy.transform.Field

@Field String newTag=''

pipeline {
    agent { label 'doa-injwn01.in.lab' }
    parameters {
        gitParameter branchFilter: 'origin/(.*)', defaultValue: 'main_1.0', name: 'BRANCH', type: 'PT_BRANCH', listSize: '10', quickFilterEnabled: true, useRepository: 'https://github.com/rajalingamp66/PipelineNuget-CustomConfigurationManager'
        choice(name: 'RELEASE_TYPE', choices: ['major', 'minor'], description: 'Select the release type for build')
    }
    environment {
      NUGET_SOURCE="https://nuget.pkg.github.com/rajalingamp66/index.json"
      GITHUB_USERNAME="Rajalingam.Periyathambi@nice.com"
      GITHUB_TOKEN=credentials("github-packages-read-write")
    }
    
    stages {        
        stage("Preparation Stage") {
            steps {
                script {
                    newTag=powershell(returnStdout: true, script: ".\\resolveGitChanges.ps1 ${RELEASE_TYPE} ${BRANCH}")
                    println "Branch name: ${BRANCH}"
                    println "New Tag generated: ${newTag}"
                }
            }
        }
        stage('Build') {
            steps {
                script {
                    echo 'Building the package'
                    withEnv(["newTag=${newTag}"]){
                        bat '''
                           
                            cd ./PipelineNuget-CustomConfigurationManager/src
                            dotnet build --configfile .nuget/NuGet.Config PipelineNuget-CustomConfigurationManager.sln
                            echo 'Packing the build'
                            cd ./PipelineNuget-CustomConfigurationManager/src
                            dotnet pack -c Release -p:Version=%newTag%

                        '''
                    }
                    echo 'Push package to Nuget'
                   
                        bat '''
                            dotnet nuget push PipelineNuget-CustomConfigurationManager/src/CustomConfigurationManager/bin/Release/PipelineNuget-CustomConfigurationManager**.nupkg -k %GITHUB_TOKEN% -s %NUGET_SOURCE%
                        ''' 
                    
                }
            }
        }
        stage('Push Tag') {
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'github-nice-cxone', usernameVariable: 'GIT_USERNAME', passwordVariable: 'GIT_PASSWORD')]) {     
                        withEnv(["newTag=${newTag}"]){
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
                currentBuild.description = "${RELEASE_TYPE} : ${newTag}"
            }
        }
    }
}
