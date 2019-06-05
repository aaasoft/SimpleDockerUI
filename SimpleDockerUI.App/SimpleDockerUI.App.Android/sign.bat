SET JAVA_HOME=C:\Program Files\Android\jdk\microsoft_dist_openjdk_1.8.0.25
SET BUILD_TOOL_PATH=C:\Program Files (x86)\Android\android-sdk\build-tools\28.0.3
"%BUILD_TOOL_PATH%\zipalign.exe" -v 4 com.github.aaasoft.SimpleDockerUI.App.apk com.github.aaasoft.SimpleDockerUI.App-Signed.apk
"%BUILD_TOOL_PATH%\apksigner.bat" sign --ks keyForApk.keystore --ks-key-alias keyForApk com.github.aaasoft.SimpleDockerUI.App-Signed.apk