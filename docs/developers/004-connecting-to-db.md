# Connecting to Konduit

## Prerequisites

### Windows Subsystem for Linux (WSL) Setup

See: [WSL Setup Guide (KB0012457)](https://dfe.service-now.com/ithelpcentre?id=kb_article&sysparm_article=KB0012457)

```bash
wsl --set-default-version 1
wsl --install -d Ubuntu
wsl --set-default Ubuntu
```

Once installed, set your UNIX username and password, then enter WSL:

```bash
wsl
```

### Azure CLI

See: [Azure CLI Installation Guide](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-linux)

```bash
curl -fsSL 'https://azurecliprod.blob.core.windows.net/$root/deb_install.sh' | sudo bash
```

#### Azure Login

```bash
az login
```

**Options:**
- If no browser available: `az login --use-device-code` and authenticate at https://login.microsoft.com/device
- Specific tenant: `az login --tenant XXX`
- When prompted, select from the available subscriptions (e.g., [10])

### System Packages

```bash
sudo apt-get update
sudo apt-get install -y make
sudo apt-get install -y postgresql-client
```

### SSL Certificate Setup

You may need to be added to the SSL certificate self-signed certificate chain exclusion group:

[Request SSL Certificate Exclusion](https://dfe.service-now.com/mydfe?id=sc_cat_item&table=sc_cat_item&sys_id=59d68b331bd13050199d6397b04bcb23&recordUrl=com.glideapp.servicecatalog_cat_item_view.do%3Fv%3D1&sysparm_id=59d68b331bd13050199d6397b04bcb23)

Ask your administrator for Azure Internet Exclude Inspection PA approval.

### Kubernetes Tools

#### kubectl

See: [kubectl Installation Guide](https://kubernetes.io/docs/tasks/tools/install-kubectl-linux/)

```bash
sudo apt-get install -y apt-transport-https ca-certificates curl gnupg
curl -fsSL https://pkgs.k8s.io/core:/stable:/v1.36/deb/Release.key | sudo gpg --dearmor -o /etc/apt/keyrings/kubernetes-apt-keyring.gpg
sudo chmod 644 /etc/apt/keyrings/kubernetes-apt-keyring.gpg

echo 'deb [signed-by=/etc/apt/keyrings/kubernetes-apt-keyring.gpg] https://pkgs.k8s.io/core:/stable:/v1.36/deb/ /' | sudo tee /etc/apt/sources.list.d/kubernetes.list
sudo chmod 644 /etc/apt/sources.list.d/kubernetes.list

sudo apt-get update
sudo apt-get install -y kubectl
```

#### kubelogin

See: [kubelogin Installation Guide](https://azure.github.io/kubelogin/install.html)

```bash
sudo az aks install-cli
```

When prompted, select the PATH option (copy the first option or set as a system variable).

#### kubectl-convert

See: [kubectl-convert Installation Guide](https://kubernetes.io/docs/tasks/tools/install-kubectl-linux/)

```bash
curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/amd64/kubectl-convert"
sudo install -o root -g root -m 0755 kubectl-convert /usr/local/bin/kubectl-convert
rm kubectl-convert kubectl-convert.sha256
```

## Setup Konduit

Once all prerequisites are installed, close your WSL session and open a new CMD window:

```bash
mkdir wsl
cd wsl
mkdir bin
```

Enter WSL:

```bash
wsl
```

Download the Makefile:

```bash
wget https://raw.githubusercontent.com/DFE-Digital/sap-public/refs/heads/main/Makefile
```

Or if using the sector repository:

```bash
wget https://raw.githubusercontent.com/DFE-Digital/sap-sector/refs/heads/main/Makefile
```

Finally, build Konduit:

```bash
make bin/konduit.sh
```



