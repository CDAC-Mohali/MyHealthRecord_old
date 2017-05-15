package com.pkg.healthrecordappname.appfinalname.modules.useables;

public class AADhaar_Profile
{
    public Long uid;
    public String name;
    public Character gender;
    public int yob; // year of birth
    public String co; // Care off
    public String house; // House address
    public String street; // Street
    public String loc; // Location
    public String vtc; //
    public String po; // Post Office
    public String dist; // District
    public String state; // State
    public Long pc; //Postal code - Pin
    public String dob;
    public String lm;

    // UID
    public Long getUID() {
        return uid;
    }

    public void setUID(Long uid) {
        this.uid = uid;
    }

    // Name
    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    // Gender
    public Character getGender() {
        return gender;
    }

    public void setGender(Character gender) {
        this.gender = gender;
    }

    // year of birth
    public Integer getYOB() {
        return yob;
    }

    public void setYOB(Integer yob) {
        this.yob = yob;
    }

    // Care off
    public String getCareoff() {
        return co;
    }

    public void setCareoff(String co) {
        this.co = co;
    }

    // House address
    public String getHouse() {
        return house;
    }

    public void setHouse(String house) {
        this.house = house;
    }

    // Street
    public String getStreet() {
        return street;
    }

    public void setStreet(String street) {
        this.street = street;
    }

    // Location
    public String getLoc() {
        return loc;
    }

    public void setLoc(String loc) {
        this.loc = loc;
    }

    // vtc;
    public String getVTC() {
        return vtc;
    }

    public void setVTC(String vtc) {
        this.vtc = vtc;
    }

    // Post Office
    public String getPostOffice() {
        return po;
    }

    public void setPostOffice(String po) {
        this.po = po;
    }

    // District
    public String getDistrict() {
        return dist;
    }

    public void setDistrict(String dist) {
        this.dist = dist;
    }

    public String getState() {
        return state;
    }

    public void setState(String state) {
        this.state = state;
    }

    // Postal Code - Pin
    public Long getPin() {
        return pc;
    }

    public void setPin(Long pc) {
        this.pc = pc;
    }

    //dob
    public String getDob() {
        return dob;
    }

    public void setDob(String dob) {
        this.dob = dob;
    }

    //lm
    public String getLM() {
        return lm;
    }

    public void setLM(String lm) {
        this.lm = lm;
    }

}
