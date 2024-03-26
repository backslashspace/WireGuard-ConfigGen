/* SPDX-License-Identifier: MIT
 *
 * Copyright (C) 2019-2021 WireGuard LLC. All Rights Reserved.
 */
package main

import (
	"C"
	"crypto/rand"
	"unsafe"

	"golang.org/x/crypto/curve25519"
)

//export WGGenKeypair
func WGGenKeypair(publicKey, privateKey *byte) {

	publicKeyArray := (*[32]byte)(unsafe.Pointer(publicKey))
	privateKeyArray := (*[32]byte)(unsafe.Pointer(privateKey))

	n, err := rand.Read(privateKeyArray[:])
	if err != nil || n != len(privateKeyArray) {
		panic("Unable to generate random bytes")
	}

	privateKeyArray[0] &= 248
	privateKeyArray[31] = (privateKeyArray[31] & 127) | 64

	curve25519.ScalarBaseMult(publicKeyArray, privateKeyArray)
}

//export WGCalcPublicKey
func WGCalcPublicKey(publicKey, privateKey *byte) {

	publicKeyArray := (*[32]byte)(unsafe.Pointer(publicKey))
	privateKeyArray := (*[32]byte)(unsafe.Pointer(privateKey))

	curve25519.ScalarBaseMult(publicKeyArray, privateKeyArray)
}

func main() {}
