-- Fix BloodDonationApplication BloodBagId references
-- This script addresses the foreign key constraint violation

-- First, let's see what BloodBagId values exist in BloodDonationApplications
-- that don't have corresponding records in BloodBags
SELECT DISTINCT bda.BloodBagId
FROM BloodDonationApplications bda
LEFT JOIN BloodBags bb ON bda.BloodBagId = bb.Id
WHERE bda.BloodBagId IS NOT NULL 
  AND bb.Id IS NULL;

-- Option 1: Set invalid BloodBagId references to NULL
UPDATE BloodDonationApplications 
SET BloodBagId = NULL 
WHERE BloodBagId IS NOT NULL 
  AND BloodBagId NOT IN (SELECT Id FROM BloodBags);

-- Option 2: Create missing BloodBag records (if you want to preserve the references)
-- This would require additional logic to determine the appropriate BloodBag properties
-- For now, we'll use Option 1 (setting to NULL)

-- Verify the fix
SELECT COUNT(*) as InvalidReferences
FROM BloodDonationApplications bda
LEFT JOIN BloodBags bb ON bda.BloodBagId = bb.Id
WHERE bda.BloodBagId IS NOT NULL 
  AND bb.Id IS NULL; 