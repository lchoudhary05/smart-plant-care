#!/bin/bash

echo "🚀 Smart Plant Care - Deployment Helper"
echo "========================================"
echo ""

# Check if git is initialized
if [ ! -d ".git" ]; then
    echo "❌ Git repository not found. Please initialize git first:"
    echo "   git init"
    echo "   git add ."
    echo "   git commit -m 'Initial commit'"
    echo "   git remote add origin <your-github-repo-url>"
    echo "   git push -u origin main"
    echo ""
    exit 1
fi

# Check if changes are committed
if [ -n "$(git status --porcelain)" ]; then
    echo "⚠️  You have uncommitted changes. Please commit them first:"
    echo "   git add ."
    echo "   git commit -m 'Prepare for deployment'"
    echo "   git push"
    echo ""
    exit 1
fi

echo "✅ Git repository is ready"
echo ""

# Build frontend
echo "🔨 Building frontend..."
cd client
if npm run build; then
    echo "✅ Frontend built successfully"
else
    echo "❌ Frontend build failed"
    exit 1
fi
cd ..

# Build backend
echo "🔨 Building backend..."
cd SmartPlantCareApi
if dotnet build -c Release; then
    echo "✅ Backend built successfully"
else
    echo "❌ Backend build failed"
    exit 1
fi
cd ..

echo ""
echo "🎉 Build completed successfully!"
echo ""
echo "📋 Next steps:"
echo "1. Push your code to GitHub:"
echo "   git push"
echo ""
echo "2. Deploy frontend to Vercel:"
echo "   - Go to vercel.com"
echo "   - Import your GitHub repository"
echo "   - Configure build settings"
echo "   - Add environment variables"
echo ""
echo "3. Deploy backend to Railway/Render:"
echo "   - Go to railway.app or render.com"
echo "   - Connect your GitHub repository"
echo "   - Add environment variables"
echo "   - Deploy"
echo ""
echo "4. Update frontend API URL in Vercel"
echo "5. Test your deployed application"
echo ""
echo "📖 See DEPLOYMENT.md for detailed instructions"
echo ""
echo "🚀 Happy deploying! 🌱✨" 