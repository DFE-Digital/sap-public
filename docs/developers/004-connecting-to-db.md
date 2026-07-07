## What worked for full install

```

wsl --set-default-version 1
wsl --install -d Ubuntu
wsl --set-default Ubuntu

as per https://dfe.service-now.com/ithelpcentre?id=kb_article&sysparm_article=KB0012457

Set UNIX username and password

-> wsl

> Install AZ CLI (as per https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-linux)
    curl -fsSL 'https://azurecliprod.blob.core.windows.net/$root/deb_install.sh' | sudo bash


az login 

az login --use-device-code (if no browser available) (https://login.microsoft.com/device)

az login --tenant XXX

then select number in [10] For example



sudo apt-get update
sudo apt-get install -y make
sudo apt-get install postgresql-client





> To continue further, may need to be added to ssl certificate - self cert in chain exclusion group
https://dfe.service-now.com/mydfe?id=sc_cat_item&table=sc_cat_item&sys_id=59d68b331bd13050199d6397b04bcb23&recordUrl=com.glideapp.servicecatalog_cat_item_view.do%3Fv%3D1&sysparm_id=59d68b331bd13050199d6397b04bcb23

ASK FOR AZURE INTERNET EXCLUDE INSPECTION PA



> Kubectl

https://kubernetes.io/docs/tasks/tools/install-kubectl-linux/

sudo apt-get install -y apt-transport-https ca-certificates curl gnupg
curl -fsSL https://pkgs.k8s.io/core:/stable:/v1.36/deb/Release.key | sudo gpg --dearmor -o /etc/apt/keyrings/kubernetes-apt-keyring.gpg
sudo chmod 644 /etc/apt/keyrings/kubernetes-apt-keyring.gpg


echo 'deb [signed-by=/etc/apt/keyrings/kubernetes-apt-keyring.gpg] https://pkgs.k8s.io/core:/stable:/v1.36/deb/ /' | sudo tee /etc/apt/sources.list.d/kubernetes.list
sudo chmod 644 /etc/apt/sources.list.d/kubernetes.list

sudo apt-get update
sudo apt-get install -y kubectl

> Kubelogin
https://azure.github.io/kubelogin/install.html

az aks install-cli

OPTIONS FOR PATH
instanced or system variable
PICK ONE, copy the first or set the second

> Kubectl convert
install kubectl-convert as per https://kubernetes.io/docs/tasks/tools/install-kubectl-linux/

curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/amd64/kubectl-convert"
sudo install -o root -g root -m 0755 kubectl-convert /usr/local/bin/kubectl-convert.
rm kubectl-convert kubectl-convert.sha256

Fin! (Done)

## Further

Once all the above is installed. Close all down and then open a new CMD window and type

```
mkdir wsl
cd wsl
mkdir bin
```

enter WSL (type WSL)

```
wget https://raw.githubusercontent.com/DFE-Digital/sap-public/refs/heads/main/Makefile || OR https://raw.githubusercontent.com/DFE-Digital/sap-sector/refs/heads/main/Makefile
```



