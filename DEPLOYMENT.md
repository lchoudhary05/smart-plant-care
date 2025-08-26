# 🚀 Smart Plant Care - Deployment Guide

## 📋 **Prerequisites**

- GitHub account
- MongoDB Atlas account (free tier)
- Vercel account (free tier)
- Railway or Render account (free tier)

## 🎯 **Deployment Strategy**

### **Frontend (React) → Vercel**

### **Backend (.NET API) → Railway/Render**

### **Database → MongoDB Atlas (already configured)**

---

## 🌐 **Step 1: Deploy Frontend to Vercel**

### **1.1 Prepare Frontend**

```bash
cd client
npm run build
```

### **1.2 Deploy to Vercel**

1. Go to [vercel.com](https://vercel.com) and sign up
2. Click "New Project"
3. Import your GitHub repository
4. Configure build settings:
   - **Framework Preset**: Create React App
   - **Build Command**: `npm run build`
   - **Output Directory**: `build`
   - **Install Command**: `npm install`

### **1.3 Environment Variables**

Add these in Vercel dashboard:

```
REACT_APP_API_URL=https://your-backend-domain.onrender.com/api
```

### **1.4 Deploy**

- Click "Deploy"
- Your app will be available at: `https://your-app.vercel.app`

---

## 🔧 **Step 2: Deploy Backend to Railway**

### **2.1 Prepare Backend**

1. Ensure your code is committed to GitHub
2. Verify `Dockerfile` and `.dockerignore` are present

### **2.2 Deploy to Railway**

1. Go to [railway.app](https://railway.app) and sign up
2. Click "New Project" → "Deploy from GitHub repo"
3. Select your repository
4. Railway will automatically detect the Dockerfile

### **2.3 Environment Variables**

Add these in Railway dashboard:

```
ConnectionStrings__MongoDB=mongodb+srv://mails4lavesh:Monu7207%40@smartcluster.faohskw.mongodb.net/?retryWrites=true&w=majority&appName=SmartCluster
JwtSettings__SecretKey=your-super-secret-production-key-here
JwtSettings__Issuer=SmartPlantCareApi
JwtSettings__Audience=SmartPlantCareApp
JwtSettings__AccessTokenExpirationMinutes=60
JwtSettings__RefreshTokenExpirationDays=7
CorsOrigins__0=https://your-frontend-domain.vercel.app
```

### **2.4 Deploy**

- Railway will build and deploy automatically
- Your API will be available at: `https://your-app.railway.app`

---

## 🔄 **Alternative: Deploy Backend to Render**

### **2.1 Deploy to Render**

1. Go to [render.com](https://render.com) and sign up
2. Click "New" → "Web Service"
3. Connect your GitHub repository
4. Configure:
   - **Name**: smart-plant-care-api
   - **Environment**: Docker
   - **Branch**: main
   - **Build Command**: (leave empty, uses Dockerfile)
   - **Start Command**: (leave empty, uses Dockerfile)

### **2.2 Environment Variables**

Add the same environment variables as Railway

### **2.3 Deploy**

- Render will build and deploy automatically
- Your API will be available at: `https://your-app.onrender.com`

---

## 🔐 **Step 3: Update Frontend API URL**

### **3.1 Update Environment Variable**

In Vercel dashboard, update:

```
REACT_APP_API_URL=https://your-backend-domain.onrender.com/api
```

### **3.2 Redeploy Frontend**

- Push changes to GitHub
- Vercel will automatically redeploy

---

## 🗄️ **Step 4: Verify MongoDB Connection**

### **4.1 Test Connection**

Your MongoDB Atlas connection should work from the deployed backend.

### **4.1 Network Access**

Ensure MongoDB Atlas allows connections from anywhere (0.0.0.0/0) or add your deployment IPs.

---

## 🧪 **Step 5: Test Deployment**

### **5.1 Test Backend**

```bash
curl https://your-backend-domain.onrender.com/api/plant/ping
# Should return: "Ping successful"
```

### **5.2 Test Frontend**

1. Visit your Vercel URL
2. Register a new user
3. Add a plant
4. Test edit/delete functionality

---

## 🔧 **Troubleshooting**

### **Common Issues:**

#### **CORS Errors**

- Ensure `CorsOrigins` includes your frontend domain
- Check that CORS middleware is configured correctly

#### **MongoDB Connection Issues**

- Verify connection string is correct
- Check MongoDB Atlas network access
- Ensure database and collections exist

#### **JWT Issues**

- Verify `SecretKey` is set and not null
- Check token expiration settings

#### **Build Failures**

- Ensure all dependencies are in `.csproj`
- Check Dockerfile syntax
- Verify `.dockerignore` excludes unnecessary files

---

## 📱 **Custom Domain (Optional)**

### **Vercel Custom Domain**

1. In Vercel dashboard, go to "Settings" → "Domains"
2. Add your custom domain
3. Configure DNS records as instructed

### **Backend Custom Domain**

- Railway/Render support custom domains
- Configure in their respective dashboards

---

## 🔒 **Security Considerations**

### **Production Checklist:**

- ✅ Use strong JWT secret key
- ✅ Enable HTTPS (automatic with Vercel/Railway/Render)
- ✅ Configure CORS properly
- ✅ Use environment variables for secrets
- ✅ Regular security updates

### **JWT Secret Key:**

Generate a strong secret:

```bash
openssl rand -base64 64
```

---

## 📊 **Monitoring & Analytics**

### **Vercel Analytics**

- Built-in performance monitoring
- Real-time analytics
- Error tracking

### **Railway/Render Monitoring**

- Built-in logs
- Performance metrics
- Error tracking

---

## 🚀 **Congratulations!**

Your Smart Plant Care SaaS is now deployed and accessible worldwide!

### **Your URLs:**

- **Frontend**: `https://your-app.vercel.app`
- **Backend**: `https://your-app.onrender.com`
- **API Docs**: `https://your-app.onrender.com/swagger`

### **Next Steps:**

1. Test all functionality
2. Set up monitoring
3. Consider adding analytics
4. Plan for scaling

---

## 💰 **Cost Breakdown (Free Tier)**

- **Vercel**: $0/month (unlimited deployments)
- **Railway**: $0/month ($5 credit)
- **Render**: $0/month (with limitations)
- **MongoDB Atlas**: $0/month (512MB)

**Total**: $0/month! 🎉

---

## 📞 **Support**

If you encounter issues:

1. Check the logs in your deployment platform
2. Verify environment variables
3. Test locally first
4. Check this guide for common solutions

**Happy Deploying! 🌱✨**
